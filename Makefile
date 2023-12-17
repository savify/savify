include makefiles/DatabaseUpdate.mk
include makefiles/Migrations.mk
include makefiles/MigrationRemoval.mk

up:
	docker-compose up -d

seed-database:
	docker cp ./src/Database/Scripts/Clear/ClearDatabase.sql savify-database:/clear.sql
	docker cp ./src/Database/Scripts/Seed/SeedDatabase.sql savify-database:/seed.sql
	docker exec -u postgres savify-database psql savify user -f /clear.sql
	docker exec -u postgres savify-database psql savify user -f /seed.sql

# You should have dotnet-format tool installed; Run dotnet tool install -g dotnet-format --version "7.*" --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json to install it.
codestyle-fix:
	dotnet format Savify.sln

codestyle-verify:
	dotnet format Savify.sln --verify-no-changes
