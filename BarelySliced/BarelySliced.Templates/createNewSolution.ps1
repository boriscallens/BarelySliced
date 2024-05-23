param (
    [string]$solutionName = $(Read-Host "Please provide a solution name"),
    [string]$domain,
    [switch]$force
)

function AddSolutionItems {
    param(
        [Parameter(Mandatory)]
        [string]$slnFilePath,
        [Parameter(Mandatory)]
        [string]$solutionFolderName,
        [Parameter(Mandatory)]
        [string[]]$files
    )

    $projectGuid = (New-Guid).ToString("B").ToUpper()
    $solutionItems = $files | ForEach-Object {
        "`t`t$_ = $_"
    } | Out-String

    $slnContent = @"
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "$solutionFolderName", "$solutionFolderName", "{$projectGuid}"
	ProjectSection(SolutionItems) = preProject
$solutionItems`tEndProjectSection
EndProject
"@

	Add-Content -Path $slnFilePath -Value $slnContent
}

function AddMissingRootNamespace($projectPath, $rootNamespace) {
    $projectFile = Get-Content -Path $projectPath
    $projectFile | ForEach-Object {
        if ($_ -match "<RootNamespace>") {
            return
        }
    }


    # Find the first propertygroup, and add the "<RootNamespace>$rootNamespace</RootNamespace>" tag.
    $projectFile | ForEach-Object {
        if ($_ -match "<PropertyGroup>") {
            $_
            "<RootNamespace>$rootNamespace</RootNamespace>"
        }
        else {
            $_
        }
    } | Set-Content -Path $projectPath
}

function AddFileTemplate($templateName, $featureName, $path){  
    ## if the template name is empty, write a helpfull message and Exit
    if([string]::IsNullOrWhiteSpace($templateName)) {
        Write-Error "Invalid template name"
        Exit 1
    }

    ## if the path is empty or does not exist, write a helpfull message and Exit
    if([string]::IsNullOrWhiteSpace($path)) {
        Write-Error "Invalid output path $path"
        Exit 1
    }

    Write-Host "Adding $templateName to $path" -ForegroundColor Yellow
    $command = "dotnet new $templateName"

    if (![string]::IsNullOrWhiteSpace($featureName)) {
        Write-Host "Using feature name $featureName" -ForegroundColor Yellow
        $command += " -n $featureName"
    }

    $command += " --output $path"

    if ($force) {
        Write-Host "Using force flag" -ForegroundColor Yellow
        $command += " --force"
    }

    Invoke-Expression -Command $command
}

$originalLocation = $PWD.Path
$folderPath = Join-Path $originalLocation $solutionName
$slnFilePath = Join-Path $folderPath "$solutionName.sln"
if ([string]::IsNullOrWhiteSpace($domain)) {
    $domain = $solutionName.Split('.')[-1]
}

if (Test-Path $folderPath -PathType Container) {
    if ($force) {
        Remove-Item $folderPath -Recurse -Force
    }
    else {
        Write-Error "The directory $solutionName already exists and is not empty. Use -force flag to proceed."
        Exit 1
    }
}

New-Item -ItemType Directory -Path $folderPath | Out-Null
Set-Location $folderPath

Write-Host "Started creating solution $solutionName" -ForegroundColor Yellow
dotnet new sln -n $solutionName
dotnet new gitignore
dotnet new editorconfig
AddSolutionItems  `
    -slnFilePath $slnFilePath `
    -solutionFolderName "Misc" `
    -files @(".editorconfig", ".gitignore")

Write-Host "Adding class libraries" -ForegroundColor Yellow
dotnet new classlib -n "$solutionName.Domain" --no-restore

dotnet new classlib -n "$solutionName.Persistence" --no-restore
dotnet add "$solutionName.Persistence/$solutionName.Persistence.csproj" reference "$solutionName.Domain/$solutionName.Domain.csproj"

dotnet new classlib -n "$solutionName.Business" --no-restore
dotnet add "$solutionName.Business/$solutionName.Business.csproj" reference "$solutionName.Persistence/$solutionName.Persistence.csproj"
dotnet add "$solutionName.Business/$solutionName.Business.csproj" reference "$solutionName.Domain/$solutionName.Domain.csproj"

dotnet new classlib -n "$solutionName.Infrastructure" --no-restore
dotnet add "$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" reference "$solutionName.Domain/$solutionName.Domain.csproj"
dotnet add "$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" reference "$solutionName.Business/$solutionName.Business.csproj"

dotnet new webapi -n "$solutionName.Api" -minimal --no-restore

dotnet sln "$solutionName.sln" add `
    "$solutionName.Domain/$solutionName.Domain.csproj" `
    "$solutionName.Business/$solutionName.Business.csproj" `
    "$solutionName.Persistence/$solutionName.Persistence.csproj" `
    "$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" `
    "$solutionName.Api/$solutionName.Api.csproj"

Write-Host "Adding test projects" -ForegroundColor Yellow
dotnet new xunit -n "$solutionName.Domain.Tests" --no-restore
dotnet add "$solutionName.Domain.Tests/$solutionName.Domain.Tests.csproj" reference "$solutionName.Domain/$solutionName.Domain.csproj"

dotnet new xunit -n "$solutionName.Business.Tests" --no-restore
dotnet add "$solutionName.Business.Tests/$solutionName.Business.Tests.csproj" reference "$solutionName.Business/$solutionName.Business.csproj"
dotnet add "$solutionName.Business.Tests/$solutionName.Business.Tests.csproj" reference "$solutionName.Persistence/$solutionName.Persistence.csproj"

dotnet new xunit -n "$solutionName.Persistence.Tests" --no-restore
dotnet add "$solutionName.Persistence.Tests/$solutionName.Persistence.Tests.csproj" reference "$solutionName.Persistence/$solutionName.Persistence.csproj"

dotnet new xunit -n "$solutionName.Infrastructure.Tests" --no-restore
dotnet add "$solutionName.Infrastructure.Tests/$solutionName.Infrastructure.Tests.csproj" reference "$solutionName.Infrastructure/$solutionName.Infrastructure.csproj"

dotnet new xunit -n "$solutionName.Api.Tests" --no-restore
dotnet add "$solutionName.Api.Tests/$solutionName.Api.Tests.csproj" reference "$solutionName.Api/$solutionName.Api.csproj"

dotnet sln "$solutionName.sln" add --solution-folder Tests `
    "$solutionName.Domain.Tests/$solutionName.Domain.Tests.csproj" `
    "$solutionName.Business.Tests/$solutionName.Business.Tests.csproj" `
    "$solutionName.Persistence.Tests/$solutionName.Persistence.Tests.csproj" `
    "$solutionName.Infrastructure.Tests/$solutionName.Infrastructure.Tests.csproj" `
    "$solutionName.Api.Tests/$solutionName.Api.Tests.csproj"

Write-Host "Setting root namespaces" -ForegroundColor Yellow
AddMissingRootNamespace "$solutionName.Domain/$solutionName.Domain.csproj" "$solutionName.Domain"
AddMissingRootNamespace "$solutionName.Persistence/$solutionName.Persistence.csproj" "$solutionName.Persistence"
AddMissingRootNamespace "$solutionName.Business/$solutionName.Business.csproj" "$solutionName.Business"
AddMissingRootNamespace "$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" "$solutionName.Infrastructure"
AddMissingRootNamespace "$solutionName.Api/$solutionName.Api.csproj" "$solutionName.Api"
AddMissingRootNamespace "$solutionName.Domain.Tests/$solutionName.Domain.Tests.csproj" "$solutionName.Domain.Tests"
AddMissingRootNamespace "$solutionName.Persistence.Tests/$solutionName.Persistence.Tests.csproj" "$solutionName.Persistence.Tests"
AddMissingRootNamespace "$solutionName.Business.Tests/$solutionName.Business.Tests.csproj" "$solutionName.Business.Tests"
AddMissingRootNamespace "$solutionName.Infrastructure.Tests/$solutionName.Infrastructure.Tests.csproj" "$solutionName.Infrastructure.Tests"
AddMissingRootNamespace "$solutionName.Api.Tests/$solutionName.Api.Tests.csproj" "$solutionName.Api.Tests"

Write-Host "Adding nuget packages" -ForegroundColor Yellow
dotnet add "$solutionName.Api/$solutionName.Api.csproj" package Microsoft.EntityFrameworkCore.Design --no-restore
dotnet add "$solutionName.Api/$solutionName.Api.csproj" package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --no-restore
dotnet add "$solutionName.Api/$solutionName.Api.csproj" package Microsoft.AspNetCore.OpenApi --prerelease --no-restore
dotnet add "$solutionName.Api/$solutionName.Api.csproj" package Microsoft.Extensions.ApiDescription.Server --prerelease --no-restore

dotnet add "$solutionName.Business/$solutionName.Business.csproj" package MediatR --no-restore
dotnet add "$solutionName.Business/$solutionName.Business.csproj" package Microsoft.AspNetCore.Authorization --no-restore

dotnet add "$solutionName.Domain/$solutionName.Domain.csproj" package StronglyTypedId --no-restore
dotnet add "$solutionName.Domain/$solutionName.Domain.csproj" package ValueOf --no-restore

dotnet add "$solutionName.Business.Tests/$solutionName.Business.Tests.csproj" package Microsoft.Extensions.DependencyInjection --no-restore
dotnet add "$solutionName.Business.Tests/$solutionName.Business.Tests.csproj" package Microsoft.EntityFrameworkCore.Sqlite --no-restore

dotnet add "$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" package MediatR --no-restore
dotnet add "$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" package Microsoft.AspNetCore.Authentication.JwtBearer --no-restore

dotnet add "$solutionName.Persistence/$solutionName.Persistence.csproj" package Azure.Identity --no-restore
dotnet add "$solutionName.Persistence/$solutionName.Persistence.csproj" package Microsoft.EntityFrameworkCore --no-restore
dotnet add "$solutionName.Persistence/$solutionName.Persistence.csproj" package Microsoft.EntityFrameworkCore.Sqlite --no-restore
dotnet add "$solutionName.Persistence/$solutionName.Persistence.csproj" package Microsoft.EntityFrameworkCore.SqlServer --no-restore
dotnet add "$solutionName.Persistence/$solutionName.Persistence.csproj" package Microsoft.Extensions.Configuration.Abstractions --no-restore
dotnet add "$solutionName.Persistence/$solutionName.Persistence.csproj" package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore --no-restore

dotnet add "$solutionName.Persistence.Tests/$solutionName.Persistence.Tests.csproj" package Microsoft.Extensions.Configuration --no-restore

dotnet restore

AddFileTemplate "bsdomainproject" $domain "$solutionName.Domain"
AddFileTemplate "bspersistenceproject" $domain "$solutionName.Persistence"
AddFileTemplate "bspersistencetestproject" $domain "$solutionName.Persistence.Tests"
AddFileTemplate "bsbusinessproject" $domain "$solutionName.Business"
AddFileTemplate "bsbusinesstestproject" $domain "$solutionName.Business.Tests"
AddFileTemplate "bsinfrastructureproject" $domain "$solutionName.Infrastructure"

Write-Host "Cleaning up placeholder files" -ForegroundColor Yellow
Get-ChildItem -Recurse -Filter "class1.cs" | Remove-Item -Force
Get-ChildItem -Recurse -Filter "unittest1.cs" | Remove-Item -Force

Write-Host "Initializing git" -ForegroundColor Yellow
git init
git add .
git commit -m "Solution Setup"

Set-Location $originalLocation
Write-Host "Solution created successfully!" -ForegroundColor Green