CREATE TABLE IF NOT EXISTS identity.users (id string PRIMARY KEY, tenant string, login_code string, hash string, salt string, 
firstname string, lastname string, email string, is_active bool, is_deleted bool, debtor_id string);

CREATE TABLE IF NOT EXISTS identity.permissionschemes (id string PRIMARY KEY, name string, client string, is_active bool);

CREATE TABLE IF NOT EXISTS identity.licences (id UUID PRIMARY KEY, name string UNIQUE);

CREATE TABLE IF NOT EXISTS identity.permissions (id UUID PRIMARY KEY, name string UNIQUE, licence_id UUID);

CREATE TABLE IF NOT EXISTS identity.roles (id UUID PRIMARY KEY, name string, permission_scheme_id string);