<#
 .SYNOPSIS
    Sets CI version build variables and/or returns version information.

 .DESCRIPTION
    The script is a wrapper around any versioning tool we use and abstracts it from
    the rest of the build system.
#>

$version = & (Join-Path $PSScriptRoot "get-version.ps1")

# Call versioning for build
& ./tools/nbgv  @("cloud", "-c", "-a")
if ($LastExitCode -ne 0) {
   Write-Warning "Error: 'nbgv cloud -c -a' failed with $($LastExitCode)."
}

# Set build environment version numbers in pipeline context
Write-Host "Setting version build variables:"

Write-Host "##vso[task.setvariable variable=Version_Full]$($version.Full)"
Write-Host "##vso[task.setvariable variable=Version_Prefix]$($version.Prefix)"
Write-Host "##vso[task.setvariable variable=Version_Prerelease]$($version.Prerelease)"

Write-Host "Version_Full=$($version.Full)"
Write-Host "Version_Prefix=$($version.Prefix)"
Write-Host "Version_Prerelease=$($version.Prerelease)"
