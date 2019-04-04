using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class seeFeatureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Features(Name) values ('Feature 1')");
            migrationBuilder.Sql("Insert into Features(Name) values ('Feature 2')");
            migrationBuilder.Sql("Insert into Features(Name) values ('Feature 3')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Features");
        }
    }
}
