[CmdletBinding()]
param (
    [ValidateSet("Ctor", "ReadValue")]
    [string]$Mode
)

$Filter = ''

switch ($Mode) {
    "Ctor" { $Filter = '*CtorBenchmark*' }
    "ReadValue" { $Filter = '*ReadValueBenchmark*' }
    Default {}
}

. dotnet run -c Release -- --job short --runtimes net7.0 --filter $Filter