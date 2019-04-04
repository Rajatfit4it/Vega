using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Makes(Name) values ('Make 1')");
            migrationBuilder.Sql("Insert into Makes(Name) values ('Make 2')");
            migrationBuilder.Sql("Insert into Makes(Name) values ('Make 3')");

            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make1 Model A',(Select Id from Makes where Name = 'Make 1'))");
            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make1 Model B',(Select Id from Makes where Name = 'Make 1'))");
            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make1 Model C',(Select Id from Makes where Name = 'Make 1'))");

            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make2 Model A',(Select Id from Makes where Name = 'Make 2'))");
            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make2 Model B',(Select Id from Makes where Name = 'Make 2'))");
            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make2 Model C',(Select Id from Makes where Name = 'Make 2'))");

            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make3 Model A',(Select Id from Makes where Name = 'Make 3'))");
            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make3 Model B',(Select Id from Makes where Name = 'Make 3'))");
            migrationBuilder.Sql("Insert into Models(Name, MakeId) values ('Make3 Model C',(Select Id from Makes where Name = 'Make 3'))");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("Delete from Models");
            migrationBuilder.Sql("Delete from Makes");
        }
    }
}
