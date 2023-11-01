using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143800, TransactionBehavior.None)]
public class AddTaskCommentV2 : Migration
{
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF EXISTS (SELECT 1 FROM pg_type WHERE typname = 'task_comments_v1') THEN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'task_comments_v2') THEN
            CREATE TYPE task_comments_v2 as (
                id                  bigint,
                task_id             bigint,
                author_user_id      bigint,
                message             text,
                at                  timestamp with time zone,
                modified_at         timestamp with time zone,
                deleted_at          timestamp with time zone
            );
        END IF;
        END IF;
    END
$$;
";
        
        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
DO $$
    BEGIN
        DROP TYPE IF EXISTS task_comments_v2;
    END
$$;
";
        
        Execute.Sql(sql);
    }
}