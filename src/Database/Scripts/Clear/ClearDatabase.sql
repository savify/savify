DO $$
    DECLARE
        schemas TEXT[];
        schema_name TEXT;
        table_row RECORD;
        sql_query TEXT;

    BEGIN
        schemas := ARRAY ['user_access', 'wallets', 'banks', 'notifications'];

        FOR schema_name IN SELECT UNNEST(schemas)
            LOOP
                FOR table_row IN
                    SELECT * FROM information_schema.tables WHERE table_schema=schema_name AND table_type='BASE TABLE'
                    LOOP
                        sql_query := 'DELETE FROM ' || schema_name || '.' || table_row.table_name;
                        EXECUTE sql_query;
                    END LOOP;
            END LOOP;

    END
$$;
