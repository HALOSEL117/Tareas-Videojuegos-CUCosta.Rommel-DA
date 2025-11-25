param(
    [string]$CsvPath = $(Join-Path $PSScriptRoot 'benchmarks.csv'),
    [string]$OutCsv = ""
)

if (-not (Test-Path $CsvPath)) { Write-Error "CSV no encontrado: $CsvPath"; exit 1 }

$csv = Import-Csv $CsvPath
$groups = $csv | Group-Object Size, Mode, Algorithm

$result = foreach ($g in $groups) {
    $sample = $g.Group[0]

    # Helper to compute average and population stddev (consistent with previous behavior)
    function Get-Stats($values) {
        $arr = $values | ForEach-Object { [double]$_ }
        if ($arr.Count -eq 0) { return @{ Avg = 0; Std = 0 } }
        $avg = ($arr | Measure-Object -Average).Average
        $sum = ($arr | ForEach-Object { ($_ - $avg) * ($_ - $avg) } | Measure-Object -Sum).Sum
        if ($arr.Count -gt 1) { $std = [math]::Sqrt($sum / $arr.Count) } else { $std = 0 }
        return @{ Avg = $avg; Std = $std }
    }

    $msStats = Get-Stats ($g.Group | ForEach-Object { [double]$_.Ms })
    $managedStats = Get-Stats ($g.Group | ForEach-Object { [double]$_.ManagedDelta })
    $privateStats = Get-Stats ($g.Group | ForEach-Object { [double]$_.PrivateDelta })
    $recDepthStats = Get-Stats ($g.Group | ForEach-Object { [double]$_.MaxRecursionDepth })

    [PSCustomObject]@{
        Size                 = [int]$sample.Size
        Mode                 = $sample.Mode
        Algorithm            = $sample.Algorithm
        AvgMs                = [math]::Round($msStats.Avg, 2)
        StdMs                = [math]::Round($msStats.Std, 2)
        AvgManagedDelta      = [math]::Round($managedStats.Avg, 0)
        StdManagedDelta      = [math]::Round($managedStats.Std, 0)
        AvgPrivateDelta      = [math]::Round($privateStats.Avg, 0)
        StdPrivateDelta      = [math]::Round($privateStats.Std, 0)
        AvgMaxRecursionDepth = [math]::Round($recDepthStats.Avg, 2)
        StdMaxRecursionDepth = [math]::Round($recDepthStats.Std, 2)
    }
}

$sorted = $result | Sort-Object Size, Mode, Algorithm

if (-not [string]::IsNullOrEmpty($OutCsv)) {
    try {
        $sorted | Export-Csv -Path $OutCsv -NoTypeInformation -Encoding UTF8
        Write-Host "Summary written to: $OutCsv"
    }
    catch {
        Write-Warning "Failed to write summary CSV: $_"
    }
}

$sorted | Format-Table -AutoSize
