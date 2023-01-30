[CmdletBinding()]
param (
    [ValidateSet('All', 'Ctor', 'ReadValue', 'Hash', 'ToString')]
    [string]$Mode
)

$Filters = @()
if ($Mode -eq 'All') {
    $ModeValues = $MyInvocation.
        MyCommand.
        Parameters["Mode"].
        Attributes.
        Where{$_ -is [System.Management.Automation.ValidateSetAttribute]}.
        ValidValues.
        Where{$_ -ne 'All'};

    foreach ($BenchName in $ModeValues) {
        $Filters += "*$($BenchName)Benchmark*"
    }
} else {
    $Filters += "*$($Mode)Benchmark*"
}

foreach ($Filter in $Filters) {
    . dotnet run -c Release -- --job short --runtimes net7.0 --filter $Filter
}

if ($Mode -eq 'All') {
    ./update-readme.ps1
}