using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143700, TransactionBehavior.None)]
public class AddColumnsToTaskCommentV1 : Migration
{
    public override void Up()
    {
        const string addColumnsSql = @"
DO $$
    BEGIN
        IF NOT EXISTS (
                SELECT 1 FROM information_schema.columns 
                WHERE table_name='task_comments'
                      AND column_name IN ('modified_at', 'deleted_at')) 
        THEN
            ALTER TABLE task_comments
            ADD COLUMN modified_at timestamp with time zone,
            ADD COLUMN deleted_at timestamp with time zone;
        END IF;
    END
$$;";
        
        Execute.Sql(addColumnsSql);
    }

    public override void Down()
    {
        const string deleteColumnsSql = @"
ALTER TABLE task_comments
DROP COLUMN modified_at,
DROP COLUMN deleted_at
;";
        
        Execute.Sql(deleteColumnsSql);
    }
}