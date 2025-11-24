$archive = Join-Path $PSScriptRoot 'benchmarks\archive'
if (Test-Path $archive) {
    Get-ChildItem -Path $archive -File -ErrorAction SilentlyContinue | ForEach-Object {
        Remove-Item -LiteralPath $_.FullName -Force -ErrorAction SilentlyContinue
        Write-Host "Removed: $($_.FullName)"
    }
}
else {
    Write-Host "Archive folder not found: $archive"
}

Write-Host 'Remaining files in archive (if any):'
Get-ChildItem -Path $archive -File -ErrorAction SilentlyContinue | ForEach-Object { Write-Host $_.FullName }
