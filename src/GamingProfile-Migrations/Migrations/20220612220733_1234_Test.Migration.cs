using FluentMigrator;

namespace GamingProfileMigrations.Migrations
{
    [Migration(20220612220733)]
    public class GP1234_Test : Migration
    {
        public override void Up()
        {          
            Execute.Script(@"Scripts/20220612220733_1234_Test_Up.sql");
        }

        public override void Down()
        {
            Execute.Script(@"Scripts/20220612220733_1234_Test_Down.sql");
        }
    }
}
