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
4. Build and run project (API)

### Available hosts
* [API - http://localhost:8080/](http://localhost:8080/)
* [Swagger](http://localhost:8080/swagger/index.html)
* [Mailcatcher - http://localhost:8888/](http://localhost:8888/)

