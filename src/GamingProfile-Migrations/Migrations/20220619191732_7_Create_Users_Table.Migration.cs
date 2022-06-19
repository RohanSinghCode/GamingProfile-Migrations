using FluentMigrator;

namespace GamingProfileMigrations.Migrations
{
    [Migration(20220619191732)]
    public class GP7_Create_Users_Table : Migration
    {
        public override void Up()
        {          
            Execute.Script(@"Scripts/20220619191732_7_Create_Users_Table_Up.sql");
        }

        public override void Down()
        {
            Execute.Script(@"Scripts/20220619191732_7_Create_Users_Table_Down.sql");
        }
    }
}
