prepare-c4:
	cd docs/C4 && git clone https://github.com/tupadr3/plantuml-icon-font-sprites.git && cd ../..
	
up:
	docker-compose up -d

# make user-access-migrations name=MigrationName
user-access-migrations:
	dotnet ef migrations add $(name) --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext

# make notifications-migrations name=MigrationName
notifications-migrations:
	dotnet ef migrations add $(name) --project src/Modules/Notifications/Infrastructure --startup-project src/API --context NotificationsContext

# make wallets-migrations name=MigrationName
wallets-migrations:
	dotnet ef migrations add $(name) --project src/Modules/Wallets/Infrastructure --startup-project src/API --context WalletsContext

# make banks-migrations name=MigrationName
banks-migrations:
	dotnet ef migrations add $(name) --project src/Modules/Banks/Infrastructure --startup-project src/API --context BanksContext
	
db-update: user-access-db-update notifications-db-update wallets-db-update banks-db-update

user-access-db-update:
	dotnet ef database update --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext

notifications-db-update:
	dotnet ef database update --project src/Modules/Notifications/Infrastructure --startup-project src/API --context NotificationsContext

wallets-db-update:
	dotnet ef database update --project src/Modules/Wallets/Infrastructure --startup-project src/API --context WalletsContext

banks-db-update:
	dotnet ef database update --project src/Modules/Banks/Infrastructure --startup-project src/API --context BanksContext

test-db-update: user-access-test-db-update notifications-test-db-update wallets-test-db-update banks-test-db-update

user-access-test-db-update:
	dotnet ef database update --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext -- --environment Testing

notifications-test-db-update:
	dotnet ef database update --project src/Modules/Notifications/Infrastructure --startup-project src/API --context NotificationsContext -- --environment Testing

wallets-test-db-update:
	dotnet ef database update --project src/Modules/Wallets/Infrastructure --startup-project src/API --context WalletsContext -- --environment Testing

banks-test-db-update:
	dotnet ef database update --project src/Modules/Banks/Infrastructure --startup-project src/API --context BanksContext -- --environment Testing

# You should have dotnet-format tool installed; Run dotnet tool install -g dotnet-format --version "7.*" --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json to install it.
codestyle-fix:
	dotnet format Savify.sln

codestyle-verify:
	dotnet format Savify.sln --verify-no-changes