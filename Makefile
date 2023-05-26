prepare-c4:
	cd docs/C4 && git clone https://github.com/tupadr3/plantuml-icon-font-sprites.git && cd ../..
	
# make user-access-migrations name=MigrationName
user-access-migrations:
	dotnet ef migrations add $(name) --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext
	
db-update: user-access-db-update

user-access-db-update:
	dotnet ef database update --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext
