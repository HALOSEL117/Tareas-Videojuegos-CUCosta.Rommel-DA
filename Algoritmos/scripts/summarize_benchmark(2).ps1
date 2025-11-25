# ==========================================
#  ANALIZADOR ROBUSTO DE BENCHMARKS (V3)
#  Corrige errores de columnas recorridas o encabezados
# ==========================================

param(
    [string]$BenchmarksDir = "",
    [string]$OutCsv = ""
)

# DETECCIÓN AUTOMÁTICA DE CARPETA
if ([string]::IsNullOrEmpty($BenchmarksDir)) {
    $subCarpeta = Join-Path $PSScriptRoot "benchmarks"
    if (Test-Path $subCarpeta) { $BenchmarksDir = $subCarpeta } 
    else { $BenchmarksDir = $PSScriptRoot }
}

Write-Host "Iniciando análisis en: $BenchmarksDir" -ForegroundColor Cyan

# 1. LEER DATOS CON SEGURIDAD
# ---------------------------------------------------------
$csvData = @()
$files = Get-ChildItem -Path $BenchmarksDir -Filter "bench_*_*.csv"

if ($files.Count -eq 0) {
    Write-Error "No se encontraron archivos bench_*_*.csv en $BenchmarksDir"
    exit 1
}

foreach ($file in $files) {
    if ($file.Name -match "bench_(\d+)_([^\.]+)\.csv") {
        $size = $matches[1]
        $mode = $matches[2]
        
        # LEEMOS EL CSV SIN FORZAR ENCABEZADOS (Dejamos que PowerShell detecte las columnas)
        $rawContent = Import-Csv $file.FullName
        
        # SI LA LECTURA FALLA (ej. no tiene headers), INTENTAMOS MODO MANUAL
        if (-not $rawContent[0].PSObject.Properties['Ticks']) {
            # Si no encuentra la columna 'Ticks', asumimos que no tiene headers y forzamos el orden común
            # Probamos si tiene 7 columnas (con Size/Mode incluidos) o 5 (solo Algoritmo...)
            $temp = Get-Content $file.FullName | Select-Object -First 1
            $cols = $temp.Split(',')
            
            if ($cols.Count -ge 6) {
                # Caso: Size, Mode, Algorithm, Ticks...
                $rawContent = Import-Csv $file.FullName -Header "ColSize","ColMode","Algorithm","Ticks","ManagedDelta","PrivateDelta","MaxRecursionDepth"
            } else {
                # Caso: Algorithm, Ticks...
                $rawContent = Import-Csv $file.FullName -Header "Algorithm","Ticks","ManagedDelta","PrivateDelta","MaxRecursionDepth"
            }
        }

        foreach ($row in $rawContent) {
            # --- FILTRO DE SEGURIDAD ---
            # 1. Si la fila es el encabezado repetido ("Algorithm" en la columna Algorithm), saltar
            if ($row.Algorithm -eq "Algorithm") { continue }
            
            # 2. Intentar parsear los Ticks. Si falla, saltar fila (es basura o header)
            $ticks = 0
            if (-not [long]::TryParse($row.Ticks, [ref]$ticks)) {
                # Si Ticks no es un número, revisamos si las columnas están recorridas
                # A veces el CSV guarda: 10000, aleatorio, QuickSort, 1234...
                if ([long]::TryParse($row.Algorithm, [ref]$ticks)) {
                    # ¡Columnas recorridas! Ajustamos manualmente
                    # (Esto pasa si el CSV no tenía headers y PS se confundió)
                } else {
                    continue # Fila inválida
                }
            }

            # Convertir a ms
            $ms = $ticks / 10000.0 

            # Parseo seguro de las otras columnas (si existen, si no 0)
            $memM = 0; [long]::TryParse($row.ManagedDelta, [ref]$memM)
            $memP = 0; [long]::TryParse($row.PrivateDelta, [ref]$memP)
            $recD = 0; [long]::TryParse($row.MaxRecursionDepth, [ref]$recD)

            $csvData += [PSCustomObject]@{
                Size              = [int]$size
                Mode              = $mode
                Algorithm         = $row.Algorithm
                Ms                = $ms
                ManagedDelta      = $memM
                PrivateDelta      = $memP
                MaxRecursionDepth = $recD
            }
        }
    }
}

# 2. ESTADÍSTICAS
# ---------------------------------------------------------
$groups = $csvData | Group-Object Size, Mode, Algorithm
$result = foreach ($g in $groups) {
    $sample = $g.Group[0]
    
    # Función estadística segura
    function Get-Stats($values) {
        $arr = $values | ForEach-Object { [double]$_ }
        if ($arr.Count -eq 0) { return @{ Avg = 0; Std = 0 } }
        $avg = ($arr | Measure-Object -Average).Average
        $sum = ($arr | ForEach-Object { [math]::Pow($_ - $avg, 2) } | Measure-Object -Sum).Sum
        if ($arr.Count -gt 1) { $std = [math]::Sqrt($sum / ($arr.Count - 1)) } else { $std = 0 }
        return @{ Avg = $avg; Std = $std }
    }

    $msStats = Get-Stats ($g.Group | ForEach-Object { [double]$_.Ms })
    $managedStats = Get-Stats ($g.Group | ForEach-Object { [double]$_.ManagedDelta })
    $recDepthStats = Get-Stats ($g.Group | ForEach-Object { [double]$_.MaxRecursionDepth })
    
    [PSCustomObject]@{
        Size                 = $sample.Size
        Mode                 = $sample.Mode
        Algorithm            = $sample.Algorithm
        Avg_Ms               = [math]::Round($msStats.Avg, 2)
        Std_Ms               = [math]::Round($msStats.Std, 2)
        Avg_ManagedBytes     = [math]::Round($managedStats.Avg, 0)
        Avg_Recursion        = [math]::Round($recDepthStats.Avg, 0)
    }
}

# 3. MOSTRAR
# ---------------------------------------------------------
$sorted = $result | Sort-Object Size, Mode, Avg_Ms

if (-not [string]::IsNullOrEmpty($OutCsv)) {
    $sorted | Export-Csv -Path $OutCsv -NoTypeInformation -Encoding UTF8
}

Write-Host "`nRESUMEN ESTADISTICO (Tiempo en ms)" -ForegroundColor Yellow
$sorted | Format-Table Size, Mode, Algorithm, Avg_Ms, Std_Ms, Avg_ManagedBytes, Avg_Recursion -AutoSize