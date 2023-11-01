using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20230917143700, TransactionBehavior.None)]
public class AddColumnsToTaskComments : Migration
{
    public override void Up()
    {
        const string sql = @"
ALTER TABLE task_comments
ADD COLUMN IF NOT EXISTS modified_at TIMESTAMP WITH TIME ZONE,
ADD COLUMN IF NOT EXISTS deleted_at  TIMESTAMP WITH TIME ZONE;
";
        
        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
ALTER TABLE task_comments
DROP COLUMN IF EXISTS modified_at,
DROP COLUMN IF EXISTS deleted_at
;";
        
        Execute.Sql(sql);

    }
}