using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143700, TransactionBehavior.None)]
public class AddTaskCommentV2Type : Migration
{
    public override void Up()
    {
        const string addFirstColumnSql = @"
ALTER TABLE task_comments
ADD COLUMN modified_at timestamp with time zone
;";
        const string addSecondColumnSql = @"
ALTER TABLE task_comments
ADD COLUMN deleted_at timestamp with time zone
;";
        
        Execute.Sql(addFirstColumnSql);
        Execute.Sql(addSecondColumnSql);
    }

    public override void Down()
    {
        const string deleteFirstColumnSql = @"
ALTER TABLE task_comments
DROP COLUMN modified_at
;";
        const string deleteSecondColumnSql = @"
ALTER TABLE task_comments
DROP COLUMN deleted_at
;";
        
        Execute.Sql(deleteFirstColumnSql);
        Execute.Sql(deleteSecondColumnSql);
    }
}