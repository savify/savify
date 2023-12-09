
-- Permissions --
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageUsers', 'ManageUsers', 'Allows to manage Users data');
INSERT INTO user_access.permissions (code, name, description) VALUES ('AddNewWallet', 'AddNewWallet', 'Allows to add new wallets');
INSERT INTO user_access.permissions (code, name, description) VALUES ('EditWallets', 'EditWallets', 'Allows to edit existing wallets');
INSERT INTO user_access.permissions (code, name, description) VALUES ('RemoveWallets', 'RemoveWallets', 'Allows to remove existing wallets');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ConnectBankAccountsToWallets', 'ConnectBankAccountsToWallets', 'Allows to connect bank account to wallets');
INSERT INTO user_access.permissions (code, name, description) VALUES ('ManageBanks', 'ManageBanks', 'Allows to manage banks');

-- Roles Permissions --
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('Admin', 'ManageUsers');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('Admin', 'ManageBanks');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'AddNewWallet');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'EditWallets');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'RemoveWallets');
INSERT INTO user_access.roles_permissions (role_code, permission_code) VALUES ('User', 'ConnectBankAccountsToWallets');

-- Users --
INSERT INTO user_access.users (id, created_at, email, is_active, name, password, preferred_language, country) VALUES ('22b1ae1a-4c4c-4ba0-b88e-a61286728c44', '2023-05-29 10:28:30.773595 +00:00', 'admin@savify.io', true, 'Savify', '$2a$12$wOi7UGAVxWm.og3kwdc1meuOoNh.f9rtBkwpnPoQs2lSeV920mkKq', 'en', null);
INSERT INTO user_access.users (id, created_at, email, is_active, name, password, preferred_language, country) VALUES ('67c31122-7efe-4397-8348-9f3d1bf22a50', '2023-06-04 10:29:32.600990 +00:00', 'user@email.com', true, 'User', '$2a$12$e24KRd72mVs5Af36wRt4sOe/xiyJIJE.S2HPAbaKOuQHdtMxUpE46', 'en', 'PL');

-- Users Roles --
INSERT INTO user_access.user_roles (role_code, user_id) VALUES ('Admin', '22b1ae1a-4c4c-4ba0-b88e-a61286728c44');
INSERT INTO user_access.user_roles (role_code, user_id) VALUES ('User', '67c31122-7efe-4397-8348-9f3d1bf22a50');

-- Users Notification Settings --
INSERT INTO notifications.user_notification_settings (id, user_id, email, name, preferred_language) VALUES ('dae48939-9cdb-42e6-8182-ff90bfb12b60', '67c31122-7efe-4397-8348-9f3d1bf22a50', 'user@email.com', 'User', 'en');

-- Wallets --
INSERT INTO finance_tracking.cash_wallets (id, user_id, balance, created_at, title, currency, updated_at, is_removed, removed_at) VALUES ('a86c8692-11ca-40c1-bc86-6dc6411c1f0b', '67c31122-7efe-4397-8348-9f3d1bf22a50', 100, '2023-08-16 14:53:52.906369 +00:00', 'New Cash wallet', 'GBP', null, false, null);
INSERT INTO finance_tracking.credit_wallets (id, user_id, available_balance, created_at, credit_limit, title, currency, updated_at, is_removed, removed_at) VALUES ('f27a9fdd-1ef0-452e-9aec-67f3a9f83e4b', '67c31122-7efe-4397-8348-9f3d1bf22a50', 10000, '2023-08-16 09:54:37.839622 +00:00', 10000, 'Credit wallet', 'USD', null, false, null);
INSERT INTO finance_tracking.debit_wallets (id, user_id, balance, created_at, title, currency, updated_at, is_removed, removed_at, bank_account_id, bank_connection_id) VALUES ('c37d08e5-0f88-4362-a49f-fc0844f994e5', '67c31122-7efe-4397-8348-9f3d1bf22a50', 1000, '2023-09-14 07:39:26.370806 +00:00', 'Debit wallet', 'PLN', null, false, null, null, null);

-- Wallets View Metadata --
INSERT INTO finance_tracking.wallet_view_metadata (wallet_id, color, icon, is_considered_in_total_balance) VALUES ('a86c8692-11ca-40c1-bc86-6dc6411c1f0b', '#000000', 'https://cdn.savify.localhost/icons/new-wallet.png', false);
INSERT INTO finance_tracking.wallet_view_metadata (wallet_id, color, icon, is_considered_in_total_balance) VALUES ('f27a9fdd-1ef0-452e-9aec-67f3a9f83e4b', '#000000', 'https://cdn.savify.localhost/icons/new-wallet.png', false);
INSERT INTO finance_tracking.wallet_view_metadata (wallet_id, color, icon, is_considered_in_total_balance) VALUES ('c37d08e5-0f88-4362-a49f-fc0844f994e5', '#ffffff', 'https://cdn.savify.localhost/icons/wallet.png', true);
