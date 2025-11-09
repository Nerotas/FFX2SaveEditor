param(
    [string]$Path = "$PSScriptRoot\..\Properties\AssemblyInfo.cs",
    [ValidateSet('revision','build')]
    [string]$Part = 'revision'
)

Write-Host "Incrementing assembly version in: $Path (part: $Part)" -ForegroundColor Cyan

if (-not (Test-Path -LiteralPath $Path)) {
    Write-Error "AssemblyInfo.cs not found at path: $Path"
    exit 1
}

$content = Get-Content -LiteralPath $Path -Raw

function Update-VersionString {
    param(
        [Parameter(Mandatory=$true)] [string]$version,
        [ValidateSet('revision','build')] [string]$part
    )
    # Expecting 4-part version: Major.Minor.Build.Revision
    $parts = $version.Split('.')
    if ($parts.Count -lt 4) {
        # Pad to 4 parts if needed
        while ($parts.Count -lt 4) { $parts += '0' }
    }

    switch ($part) {
        'build'    { $idx = 2 }
        'revision' { $idx = 3 }
    }

    if (-not [int]::TryParse($parts[$idx], [ref]([int]$null))) {
        # If not numeric (e.g., '*'), reset to 0
        $parts[$idx] = '0'
    }

    $parts[$idx] = ([int]$parts[$idx] + 1).ToString()
    return ($parts -join '.')
}

# Update AssemblyVersion
$assemblyVersionPattern = '\[assembly:\s*AssemblyVersion\("(?<ver>\d+\.\d+\.\d+\.\d+)"\)\]'
$m1 = [System.Text.RegularExpressions.Regex]::Match($content, $assemblyVersionPattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase -bor [System.Text.RegularExpressions.RegexOptions]::Multiline)
if ($m1.Success) {
    $old1 = $m1.Groups['ver'].Value
    $new1 = Update-VersionString -version $old1 -part $Part
        $oldLine1 = $m1.Value
        $newLine1 = '[assembly: AssemblyVersion("' + $new1 + '")]'
        $content = $content.Replace($oldLine1, $newLine1)
}

# Update AssemblyFileVersion
$fileVersionPattern = '\[assembly:\s*AssemblyFileVersion\("(?<ver>\d+\.\d+\.\d+\.\d+)"\)\]'
$m2 = [System.Text.RegularExpressions.Regex]::Match($content, $fileVersionPattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase -bor [System.Text.RegularExpressions.RegexOptions]::Multiline)
if ($m2.Success) {
    $old2 = $m2.Groups['ver'].Value
    $new2 = Update-VersionString -version $old2 -part $Part
        $oldLine2 = $m2.Value
        $newLine2 = '[assembly: AssemblyFileVersion("' + $new2 + '")]'
        $content = $content.Replace($oldLine2, $newLine2)
}

[System.IO.File]::WriteAllText($Path, $content, [System.Text.Encoding]::UTF8)

Write-Host "Version increment complete." -ForegroundColor Green

# Also mirror the version to Version.props for packaging purposes
try {
    $verPattern = '\[assembly:\s*AssemblyFileVersion\("(?<ver>\d+\.\d+\.\d+\.\d+)"\)\]'
    $m = [System.Text.RegularExpressions.Regex]::Match($content, $verPattern)
    if ($m.Success) {
        $version = $m.Groups['ver'].Value
        $propsPath = Join-Path $PSScriptRoot '..\Version.props'
        $xml = @(
            '<?xml version="1.0" encoding="utf-8"?>',
            '<Project>',
            '  <PropertyGroup>',
            "    <AppVersion>$version</AppVersion>",
            '  </PropertyGroup>',
            '</Project>'
        ) -join "`r`n"
        Set-Content -Path $propsPath -Value $xml -Encoding UTF8
        Write-Host "Synchronized AppVersion in Version.props -> $version" -ForegroundColor Green
    }
} catch {
    Write-Warning "Failed to update Version.props: $($_.Exception.Message)"
}
