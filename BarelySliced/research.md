# Solution templating language
## Why
Create features faster and completer
Nudge developers to implement patterns and practices
Reduce time spent with boilerplate
Make it easy to jump in to a new solution through uniformity

## Problems to solve
Be IDE agnostic (VS, Rider)
Be multiplatform (linux, windows, mac)
We need multi project templating that is aware of the decisions made
Be future proof
Maintenance should not be more complex then the problem

## How
### Set up an example standard solution
Be opinionated and explicit.
This can be used as a way of documenting solution practices
It can be used for demo purposes
Can be referenced during PR conversations

### Solution template
A powershell that
  - creates a sln
  - adds solution files like editorconfig, gitignore etc
  - adds domain, business, persistence, infra, api and their test projects
  - adds correct project references
  - adds nugets we depend on
  - uses project templates on the created projects
  - intializes git

### Project template
A powershell that
  - prepares a certain project for its purpose
  - adds specific files to the solution
  - uses file templates

### Files
Nuget new templates that add a specif (set of) files to a project

### Feature script
Creates required files to all the projects to support a new features
  - business
    - handler
	- request
	- response
	- validator
	- documentation
	- metric
  - business.test
    - handlershould
	- validatorshould

## How to use
In each csproj are build steps that copy the tempalate.json and the required files to the right directory.
When in debug it also installs the nuget package so the dotnet new template can immediately be tested.
There is no production flow yet

## How to extend the template solution
When changing nuget references or project references don't forget to update createNewSolution.ps1 in the BarelySliced.Templates folder.
Dotnet new file templates are really just dumb template replacements and don't know anything about project or solution structure.
For that we use them in powershell scripts to facilitate multi-project aditions or more complex tasks.

## Terms used
For template replacement purposes I used the word Sliver to denote the domain the solution solves.
BarelySliced is used to find the company name.
We assume the second word in the namespace is always the domain.

## Principles
The createnewsolution script always uses the latest nuget packages and classlibs.
This hopefully keeps chores like updating nuget packages to a minimum.
It has the downside that the solution might not be stable, but that is a tradeoff I'm willing to make.