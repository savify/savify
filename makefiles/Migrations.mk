# make [module-name]-migrations name=MigrationName
user-access-migrations:
	dotnet ef migrations add $(name) --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext

notifications-migrations:
	dotnet ef migrations add $(name) --project src/Modules/Notifications/Infrastructure --startup-project src/API --context NotificationsContext

finance-tracking-migrations:
	dotnet ef migrations add $(name) --project src/Modules/FinanceTracking/Infrastructure --startup-project src/API --context FinanceTrackingContext

banks-migrations:
	dotnet ef migrations add $(name) --project src/Modules/Banks/Infrastructure --startup-project src/API --context BanksContext

categories-migrations:
	dotnet ef migrations add $(name) --project src/Modules/Categories/Infrastructure --startup-project src/API --context CategoriesContext

transactions-migrations:
	dotnet ef migrations add $(name) --project src/Modules/Transactions/Infrastructure --startup-project src/API --context TransactionsContext
