
-- Permissions --
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageUsers', 'ManageUsers', 'Allows to manage Users data');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageWallets', 'ManageWallets', 'Allows to manage Wallets');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageTransfers', 'ManageTransfers', 'Allows to manage Transfers');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageExpenses', 'ManageExpenses', 'Allows to manage Expenses');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageIncomes', 'ManageIncomes', 'Allows to manage Incomes');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ConnectBankAccountsToWallets', 'ConnectBankAccountsToWallets', 'Allows to connect bank account to wallets');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageBanks', 'ManageBanks', 'Allows to manage banks');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageCategories', 'ManageCategories', 'Allows to manage categories');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageCustomCategories', 'ManageCustomCategories', 'Allows to manage custom categories');

-- Roles Permissions --
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('Admin', 'ManageUsers');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('Admin', 'ManageBanks');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('Admin', 'ManageCategories');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'ManageWallets');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'ManageTransfers');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'ManageExpenses');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'ManageIncomes');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'ConnectBankAccountsToWallets');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'ManageCustomCategories');

-- Users --
INSERT INTO user_access.users (id, created_at, email, is_active, name, password, preferred_language, country) VALUES ('22b1ae1a-4c4c-4ba0-b88e-a61286728c44', '2023-05-29 10:28:30.773595 +00:00', 'admin@savify.io', true, 'Savify', 'AAiunJOzFXPFnEP7TCA8b7WfX3b7vpUavMjWJRk3KDf2VAsa2sZHdU33UkkL7yGLrg==', 'en', 'PL');
INSERT INTO user_access.users (id, created_at, email, is_active, name, password, preferred_language, country) VALUES ('67c31122-7efe-4397-8348-9f3d1bf22a50', '2023-06-04 10:29:32.600990 +00:00', 'user@email.com', true, 'User', 'AIYCEV3p1AHowVqoQvb2AVDuAhdeUBcMk/suDV3FT4JVMbCw6pzGnbdbw2D210dZnQ==', 'en', 'PL');

-- Users Roles --
INSERT INTO user_access.user_roles (role_code, user_id) VALUES ('Admin', '22b1ae1a-4c4c-4ba0-b88e-a61286728c44');
INSERT INTO user_access.user_roles (role_code, user_id) VALUES ('User', '67c31122-7efe-4397-8348-9f3d1bf22a50');

-- Users Notification Settings --
INSERT INTO notifications.user_notification_settings (id, user_id, email, name, preferred_language) VALUES ('dae48939-9cdb-42e6-8182-ff90bfb12b60', '67c31122-7efe-4397-8348-9f3d1bf22a50', 'user@email.com', 'User', 'en');

-- Wallets --
INSERT INTO finance_tracking.cash_wallets (id, user_id, initial_balance, title, currency, is_removed) VALUES ('a86c8692-11ca-40c1-bc86-6dc6411c1f0b', '67c31122-7efe-4397-8348-9f3d1bf22a50', 100, 'New Cash wallet', 'GBP', false);
INSERT INTO finance_tracking.wallet_histories (wallet_id) VALUES ('a86c8692-11ca-40c1-bc86-6dc6411c1f0b');
INSERT INTO finance_tracking.credit_wallets (id, user_id, initial_available_balance, credit_limit, title, currency, is_removed) VALUES ('f27a9fdd-1ef0-452e-9aec-67f3a9f83e4b', '67c31122-7efe-4397-8348-9f3d1bf22a50', 10000, 10000, 'Credit wallet', 'USD', false);
INSERT INTO finance_tracking.wallet_histories (wallet_id) VALUES ('f27a9fdd-1ef0-452e-9aec-67f3a9f83e4b');
INSERT INTO finance_tracking.debit_wallets (id, user_id, initial_balance, title, currency, is_removed, bank_account_id, bank_connection_id) VALUES ('c37d08e5-0f88-4362-a49f-fc0844f994e5', '67c31122-7efe-4397-8348-9f3d1bf22a50', 1000, 'Debit wallet', 'PLN', false, null, null);
INSERT INTO finance_tracking.wallet_histories (wallet_id) VALUES ('c37d08e5-0f88-4362-a49f-fc0844f994e5');

-- Wallets View Metadata --
INSERT INTO finance_tracking.wallet_view_metadata (wallet_id, color, icon, is_considered_in_total_balance) VALUES ('a86c8692-11ca-40c1-bc86-6dc6411c1f0b', '#000000', 'https://cdn.savify.localhost/icons/new-wallet.png', false);
INSERT INTO finance_tracking.wallet_view_metadata (wallet_id, color, icon, is_considered_in_total_balance) VALUES ('f27a9fdd-1ef0-452e-9aec-67f3a9f83e4b', '#000000', 'https://cdn.savify.localhost/icons/new-wallet.png', false);
INSERT INTO finance_tracking.wallet_view_metadata (wallet_id, color, icon, is_considered_in_total_balance) VALUES ('c37d08e5-0f88-4362-a49f-fc0844f994e5', '#ffffff', 'https://cdn.savify.localhost/icons/wallet.png', true);

-- Users Finance Tracking Settings --
INSERT INTO finance_tracking.user_finance_tracking_settings (id, user_id, currency) VALUES ('fa154d22-4deb-44b7-9d5c-aac63a30410a', '67c31122-7efe-4397-8348-9f3d1bf22a50', 'USD');
