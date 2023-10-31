using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143700, TransactionBehavior.None)]
public class AddColumnsToTaskCommentV1 : Migration
{
    public override void Up()
    {
        const string addColumnsSql = @"
ALTER TABLE task_comments
ADD COLUMN IF NOT EXISTS modified_at TIMESTAMP WITH TIME ZONE,
ADD COLUMN IF NOT EXISTS deleted_at  TIMESTAMP WITH TIME ZONE;
";
        
        Execute.Sql(addColumnsSql);
    }

    public override void Down()
    {
        const string deleteColumnsSql = @"
ALTER TABLE task_comments
DROP COLUMN IF EXISTS modified_at,
DROP COLUMN IF EXISTS deleted_at
;";
        
        Execute.Sql(deleteColumnsSql);
    }
}