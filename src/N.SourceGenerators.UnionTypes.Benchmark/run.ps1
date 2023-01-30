[CmdletBinding()]
param (
    [ValidateSet("Ctor", "ReadValue", "Hash")]
    [string]$Mode
)

$Filter = ''

switch ($Mode) {
    "Ctor" { $Filter = '*CtorBenchmark*' }
    "ReadValue" { $Filter = '*ReadValueBenchmark*' }
    "Hash" { $Filter = '*HashBenchmark*' }
    Default {}
}

. dotnet run -c Release -- --job short --runtimes net7.0 --filter $Filter