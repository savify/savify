user-access-migration-removal:
	dotnet ef migrations remove --project src/Modules/UserAccess/Infrastructure --startup-project src/API --context UserAccessContext

notifications-migration-removal:
	dotnet ef migrations remove --project src/Modules/Notifications/Infrastructure --startup-project src/API --context NotificationsContext

finance-tracking-migration-removal:
	dotnet ef migrations remove --project src/Modules/FinanceTracking/Infrastructure --startup-project src/API --context FinanceTrackingContext

banks-migration-removal:
	dotnet ef migrations remove --project src/Modules/Banks/Infrastructure --startup-project src/API --context BanksContext

categories-migration-removal:
	dotnet ef migrations remove --project src/Modules/Categories/Infrastructure --startup-project src/API --context CategoriesContext
