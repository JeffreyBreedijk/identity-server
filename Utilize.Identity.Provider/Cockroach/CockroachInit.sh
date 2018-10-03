#!/bin/sh

host=utilizeidentityprovider_cockroach_1

./cockroach sql --insecure --host="$host" --execute="  DROP DATABASE IF EXISTS indentity CASCADE; \
                                        CREATE DATABASE IF NOT EXISTS identity; \
                                        CREATE USER IF NOT EXISTS utilize; \
                                        GRANT ALL ON DATABASE identity TO utilize;"
                                        