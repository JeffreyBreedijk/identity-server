CREATE TABLE IF NOT EXISTS identity.users (id string PRIMARY KEY, tenant string, login_code string, hash string, salt string, 
firstname string, lastname string, email string, is_active bool, is_deleted bool, debtor_id string);

CREATE TABLE IF NOT EXISTS identity.permissionschemes (id string PRIMARY KEY, name string, tenant string, is_deleted bool);

