Get-ChildItem "./Snapshots/*.received.*" |
    ForEach-Object {
        $Destination = $_.FullName -replace 'received', 'verified'
        Move-Item -Path $_.FullName -Destination $Destination -Force -PassThru -ErrorAction silentlyContinue
    }
