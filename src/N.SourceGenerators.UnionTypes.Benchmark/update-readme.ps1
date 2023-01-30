Get-ChildItem -File -Filter '*github.md' ./BenchmarkDotNet.Artifacts/results/ |
    Sort-Object Name |
    ForEach-Object {
        $BenchmarkName = $_.Name -replace '([\w\.]+)\.(\w+)-report-github\.md', '$2'
        $BenchmarkResult = Get-Content $_.FullName

        "## $BenchmarkName"
        $BenchmarkResult
        ""
    } |
    Join-String -Separator "`r`n" |
    Set-Content -Path './README.md'