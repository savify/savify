# Empower Your Finances with Savify: Track, Achieve, and Thrive!

## Local development

### Requirements
1. [Docker](https://docker.com/) and [docker-compose](https://docs.docker.com/compose/install/)
2. [.Net CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/) and [EF Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

### How to set up project for local development
1. Clone Git repository
    ```bash
    git clone git@github.com:savify/savify.git
    ```
2. Run docker containers
    ```bash
    make up
    ```
3. Run database migrations
    ```bash
    make db-update
    ```
4. Seed database with initial data
    ```bash
    make seed-database
    ```
5. For making API calls to external providers, you should set all required [secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=linux) for a project. Run
    ```bash
   dotnet user-secrets set "SaltEdge:AppId" "**AppId**" --project src/API
   dotnet user-secrets set "SaltEdge:AppSecret" "**AppSecret**" --project src/API
   ```
6. Build and run project (API)

### Available hosts
* [API - http://localhost:8080/](http://localhost:8080/)
* [Swagger](http://localhost:8080/swagger/index.html)
* [Mailcatcher - http://localhost:8888/](http://localhost:8888/)
* [Adminer - http://localhost:8880/](http://localhost:8880/)
* [Webhook Catcher - webhook.site](https://webhook.site/#!/eaf8199c-24b6-4d29-8f24-2781def88187/)

## Contributing
All commits, branches and pull requests should be named according to [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specifications.

Example:
* You have a task named SAV-1 "Initialize User Access module"
* You create a branch with name `feature/SAV-1-user-access-module-init`
* Commit messages:
  * feat(SAV-1): add domain project
  * chore(SAV-1): add application project
  * chore(SAV-1): add infrastructure project
  * test(SAV-1): add unit and integration tests
  * refactor(SAV-1): module cleanup
* Pull request from `feature/SAV-1-user-access-module-init` to `main` branch: `feat(SAV-1): user access module initialization`
* When merging pull request remember about squashing commits from your branch.

### Code standards
We use `.editorconfig` to ensure the same code standards on all local environments. Make sure you have `dotnet-format`
tool installed; Run:
```
dotnet tool install -g dotnet-format --version "7.*" --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json
```
to install it. Before each commit run `make codestyle-fix` to align all changed files to .editorconfig settings.

