db-update: user-access-db-update notifications-db-update finance-tracking-db-update banks-db-update categories-db-update transactions-db-update

user-access-db-update:
	dotnet ef database update --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext

notifications-db-update:
	dotnet ef database update --project src/Modules/Notifications/Infrastructure --startup-project src/API --context NotificationsContext

finance-tracking-db-update:
	dotnet ef database update --project src/Modules/FinanceTracking/Infrastructure --startup-project src/API --context FinanceTrackingContext

banks-db-update:
	dotnet ef database update --project src/Modules/Banks/Infrastructure --startup-project src/API --context BanksContext

categories-db-update:
	dotnet ef database update --project src/Modules/Categories/Infrastructure --startup-project src/API --context CategoriesContext

transactions-db-update:
	dotnet ef database update --project src/Modules/Transactions/Infrastructure --startup-project src/API --context TransactionsContext
