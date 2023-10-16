DELETE FROM banks.bank_revisions;
DELETE FROM banks.banks;
DELETE FROM banks.banks_synchronisation_processes;
DELETE FROM banks.inbox_messages;
DELETE FROM banks.internal_commands;
DELETE FROM banks.outbox_messages;


DELETE FROM notifications.inbox_messages;
DELETE FROM notifications.internal_commands;
DELETE FROM notifications.user_notification_settings;


DELETE FROM user_access.inbox_messages;
DELETE FROM user_access.internal_commands;
DELETE FROM user_access.outbox_messages;
DELETE FROM user_access.password_reset_requests;
DELETE FROM user_access.permissions;
DELETE FROM user_access.refresh_tokens;
DELETE FROM user_access.roles_permissions;
DELETE FROM user_access.user_registrations;
DELETE FROM user_access.user_roles;
DELETE FROM user_access.users;


DELETE FROM wallets.assets;
DELETE FROM wallets.bank_accounts;
DELETE FROM wallets.bank_connection_processes;
DELETE FROM wallets.bank_connections;
DELETE FROM wallets.cash_wallets;
DELETE FROM wallets.credit_wallets;
DELETE FROM wallets.debit_wallets;
DELETE FROM wallets.inbox_messages;
DELETE FROM wallets.internal_commands;
DELETE FROM wallets.investment_portfolios;
DELETE FROM wallets.outbox_messages;
DELETE FROM wallets.portfolio_view_matadata;
DELETE FROM wallets.salt_edge_connections;
DELETE FROM wallets.salt_edge_customers;
DELETE FROM wallets.wallet_view_metadata;
