using Microsoft.EntityFrameworkCore.Migrations; 

namespace userVoice.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up( MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              Insert into AspNetRoles(Id, [name], [NormalizedName])
              values ('c6894717-a803-4c30-a7c5-34d723ef98c8', 'Admin' , 'Admin'   )
             "); 
        }
    }
}
