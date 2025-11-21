param(
    [int[]] $Sizes = @(1000, 5000, 10000),
    [string[]] $Modes = @("aleatorio", "cruel"),
    [int] $Repeats = 3,
    [string] $OutFile = "benchmarks.csv"
)

# Asegurarse de estar en la carpeta del script
Set-Location -Path $PSScriptRoot

Write-Host "Compilando proyecto..."
dotnet build .\Algoritmos.csproj | Out-Null

if (Test-Path $OutFile) { Remove-Item $OutFile }

# Cabecera CSV (el programa puede crearla si se llama con --out, pero la pre-creamos aquí con BOM)
$bomEnc = New-Object System.Text.UTF8Encoding $true
[System.IO.File]::WriteAllText($OutFile, "Size,Mode,Run,Algorithm,Ticks,Ms`r`n", $bomEnc)

foreach ($size in $Sizes) {
    foreach ($mode in $Modes) {
        Write-Host "Ejecutando size=$size mode=$mode repeats=$Repeats"
        # Pasamos flag --headless, --repeat y --out para que el programa ejecute internamente varias repeticiones
        $argList = @(
            'run',
            '--project', '.\Algoritmos.csproj',
            '--',
            $size.ToString(),
            $mode.ToString(),
            '--repeat', $Repeats.ToString(),
            '--headless',
            '--out', $OutFile
        )
        Start-Process -FilePath 'dotnet' -ArgumentList $argList -NoNewWindow -Wait -PassThru | Out-Null
    }
}

Write-Host "Benchmarks completados. Resultados guardados en $OutFile"
