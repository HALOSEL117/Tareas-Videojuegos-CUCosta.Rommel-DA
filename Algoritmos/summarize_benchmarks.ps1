$csvPath = Join-Path $PSScriptRoot 'benchmarks.csv'
if (-not (Test-Path $csvPath)) { Write-Error "benchmarks.csv no encontrado en $PSScriptRoot"; exit 1 }

$csv = Import-Csv $csvPath
$groups = $csv | Group-Object Size,Mode,Algorithm

$result = foreach ($g in $groups) {
    $sample = $g.Group[0]
    $arr = $g.Group | ForEach-Object {[double]$_.Ms}
    $avg = ($arr | Measure-Object -Average).Average
    $sum = ($arr | ForEach-Object {($_ - $avg)*($_ - $avg)} | Measure-Object -Sum).Sum
    if ($arr.Count -gt 1) { $std = [math]::Sqrt($sum / $arr.Count) } else { $std = 0 }
    [PSCustomObject]@{
        Size = [int]$sample.Size
        Mode = $sample.Mode
        Algorithm = $sample.Algorithm
        AvgMs = [math]::Round($avg,2)
        StdMs = [math]::Round($std,2)
    }
}

$result | Sort-Object Size,Mode,Algorithm | Format-Table -AutoSize
