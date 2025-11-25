param(
    [int[]] $Sizes = @(1000, 5000, 10000),
    [string[]] $Modes = @("aleatorio", "cruel"),
    [int] $Repeats = 3,
    [string] $OutFile = "benchmarks\benchmarks.csv"
)

# Asegurarse de estar en la carpeta del script
Set-Location -Path $PSScriptRoot

# Preparar carpeta de benchmarks y mover CSV existente (si existe)
$benchDir = Join-Path $PSScriptRoot 'benchmarks'
New-Item -ItemType Directory -Path $benchDir -Force | Out-Null

if (-not [string]::IsNullOrEmpty($OutFile)) {
    if ([System.IO.Path]::IsPathRooted($OutFile)) {
        $outFull = $OutFile
    }
    else {
        $outFull = Join-Path $PSScriptRoot $OutFile
    }
    $archiveDir = Join-Path $benchDir 'archive'
    New-Item -ItemType Directory -Path $archiveDir -Force | Out-Null
    if (Test-Path $outFull) {
        $ts = Get-Date -Format "yyyyMMdd_HHmmss"
        $name = Split-Path $outFull -Leaf
        $dest = Join-Path $archiveDir ("{0}_{1}" -f $ts, $name)
        Move-Item -Path $outFull -Destination $dest -Force
        Write-Host "Archived existing file: $outFull -> $dest"
    }
}

Write-Host "Compilando proyecto..."
dotnet build .\Algoritmos.csproj | Out-Null


# Eliminar el archivo de salida si existe; CsvWriter se encargará de crear el encabezado con BOM
if (Test-Path $outFull) { Remove-Item $outFull }

foreach ($size in $Sizes) {
    foreach ($mode in $Modes) {
        Write-Host "Ejecutando size=$size mode=$mode repeats=$Repeats"
        # Construimos la linea de argumentos como string y aseguramos que el path de salida esté entrecomillado
        $escapedOut = '"' + $outFull + '"'
        $args = "run --project .\Algoritmos.csproj -- $size $mode --repeat $Repeats --headless --out $escapedOut"
        Start-Process -FilePath 'dotnet' -ArgumentList $args -NoNewWindow -Wait -PassThru | Out-Null
    }
}

Write-Host "Benchmarks completados. Resultados guardados en $OutFile"

# Generar resumen automático y guardarlo en benchmarks\summary.csv
$summaryPath = Join-Path $benchDir 'summary.csv'
try {
    $summScript = Join-Path $PSScriptRoot 'summarize_benchmarks.ps1'
    if (Test-Path $summScript) {
        Write-Host "Generando resumen estadístico en: $summaryPath"
        & $summScript -CsvPath $outFull -OutCsv $summaryPath
    }
    else {
        Write-Warning "No se encontró summarize_benchmarks.ps1 en $PSScriptRoot. Saltando resumen."
    }
}
catch {
    Write-Warning "Error al generar el resumen: $_"
}
