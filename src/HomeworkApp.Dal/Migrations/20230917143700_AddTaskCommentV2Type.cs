using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143700, TransactionBehavior.None)]
public class AddTaskCommentV2Type : Migration
{
    public override void Up()
    {
        const string addColumnsSql = @"
ALTER TABLE task_comments
ADD COLUMN modified_at timestamp with time zone,
ADD COLUMN deleted_at timestamp with time zone
;";
        
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