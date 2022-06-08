using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class addManyToManyBookAndAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fluent_BookAuthor",
                columns: table => new
                {
                    Fluent_Book_Id = table.Column<int>(type: "int", nullable: false),
                    Fluent_Author_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fluent_BookAuthor", x => new { x.Fluent_Author_Id, x.Fluent_Book_Id });
                    table.ForeignKey(
                        name: "FK_Fluent_BookAuthor_Fluent_Author_Fluent_Author_Id",
                        column: x => x.Fluent_Author_Id,
                        principalTable: "Fluent_Author",
                        principalColumn: "Author_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fluent_BookAuthor_Fluent_Book_Fluent_Book_Id",
                        column: x => x.Fluent_Book_Id,
                        principalTable: "Fluent_Book",
                        principalColumn: "Book_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookAuthor_Fluent_Book_Id",
                table: "Fluent_BookAuthor",
                column: "Fluent_Book_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fluent_BookAuthor");
        }
    }
}
