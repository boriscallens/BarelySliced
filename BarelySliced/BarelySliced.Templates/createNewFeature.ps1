param(
    [Parameter(Mandatory=$true)]
    [string]$featureName = $(Read-Host "Please provide a feature name"),

    [Parameter(Mandatory=$false)]
    [string]$solutionName = "",

    [switch]$force
)

function FindSolutionPath($solutionName) {
    $solutions = Get-ChildItem -Path .\ -Filter *.sln -Recurse -File | Select-Object -ExpandProperty FullName
    $solutionPath = $null

    if ($solutions.Count -eq 0) {
        Write-Error "No solutions found." -ErrorAction SilentlyContinue
        exit
    }
    if ($solutions.Count -gt 1 -and !$solutionName) {
        Write-Error "Multiple solutions found. Please use the -solutionName parameter to specify the solution." -ErrorAction SilentlyContinue
        exit
    }

    # Pick the solution that matches the solution name or the single solution if no name is provided
    $solutionPath = $solutions | Where-Object { (Split-Path $_ -Leaf) -like "*$solutionName*" }
    if (!$solutionPath) {
        Write-Error "Solution '$solutionName' not found." -ErrorAction SilentlyContinue
        exit
    }
    return $solutionPath
}

function FindProjectPath($solutionPath, $projectPattern) {
    # Read the .sln file
    $slnContent = Get-Content -Path $solutionPath

    # Find the line that contains the project pattern
    $projectLine = $slnContent | Where-Object { $_ -like "*$projectPattern*" }

    if (!$projectLine) {
        Write-Error "No project matching '$projectPattern' found in the solution." -ErrorAction SilentlyContinue
        exit
    }

    # Extract the project path from the line
    $projectPath = $projectLine -split '"' | Where-Object { $_ -like "*$projectPattern*" }

    # If the path is relative, convert it to absolute
    # Write-Host "Converting path to absolute: $projectPath" -ForegroundColor DarkGray
    # Write-Host "Joining $solutionPath with $projectPath" -ForegroundColor DarkGray
    # Write-Host "Path is now $projectPath" -ForegroundColor DarkGray
    # [System.IO.Path]::GetFullPath($solutionPath, $projectPath)
    # Join-Path -Path (Split-Path -Path $solutionPath) -ChildPath $projectPath
    # if (!(Test-Path -Path $projectPath)) {
    #     Write-Host "Path is relative." -ForegroundColor DarkGray
    #     Write-Host "Joining $solutionPath with $projectPath" -ForegroundColor DarkGray
    #     $projectPath = Join-Path -Path (Split-Path -Path $solutionPath) -ChildPath $projectPath
    #     Write-Host "Path is now $projectPath" -ForegroundColor DarkGray
    # }

    return $projectPath
}

function FindFullPath($solutionPath, $projectPath) {
    $fullPath = Join-Path -Path (Split-Path -Path $solutionPath) -ChildPath $projectPath
    $directoryPath = (Get-Item $fullPath).DirectoryName
    return $directoryPath
}

function AddFileTemplate($templateName, $path){
    if($force) {
        Write-Host "Force creating $templateName files at $path" -ForegroundColor Yellow
        dotnet new $templateName -n $featureName -o "$path" --force
    }else {
        Write-Host "Creating $templateName files at $path" -ForegroundColor Cyan
        dotnet new $templateName -n $featureName -o "$path"
    }
}

$slnPath = FindSolutionPath $solutionName
$businessProjectPath = FindProjectPath $slnPath "*.Business.csproj"
$businessTestProjectPath = FindProjectPath $slnPath "*.Business.Tests.csproj"
$apiProjectPath = FindProjectPath $slnPath "*.Api.csproj"
$apiTestProjectPath = FindProjectPath $slnPath "*.Api.Tests.csproj"

$businessPath = FindFullPath $slnPath $businessProjectPath
$businessTestPath = FindFullPath $slnPath $businessTestProjectPath
$apiPath = FindFullPath $slnPath $apiProjectPath
$apiTestPath = FindFullPath $slnPath $apiTestProjectPath

Write-Host "Found solution " -ForegroundColor Cyan -NoNewline; Write-Host $slnPath -ForegroundColor White
Write-Host "with" -ForegroundColor Cyan
Write-Host ("    {0,-15}" -f "* business") -NoNewline; Write-Host " $businessProjectPath" -ForegroundColor DarkGray
Write-Host ("    {0,-15}" -f "* business test") -NoNewline; Write-Host " $businessTestProjectPath" -ForegroundColor DarkGray
Write-Host ("    {0,-15}" -f "* api") -NoNewline; Write-Host " $apiProjectPath" -ForegroundColor DarkGray
Write-Host ("    {0,-15}" -f "* api test") -NoNewline; Write-Host " $apiTestProjectPath" -ForegroundColor DarkGray
Write-Host

AddFileTemplate "bsfeature-business" $businessPath
AddFileTemplate "bsfeature-tests" $businessTestPath