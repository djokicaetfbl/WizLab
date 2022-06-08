﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AddOneToManyFluentBookAndPublisher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fluent_Publisher_Id",
                table: "Fluent_Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_Book_Fluent_Publisher_Id",
                table: "Fluent_Book",
                column: "Fluent_Publisher_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_Book_Fluent_Publisher_Fluent_Publisher_Id",
                table: "Fluent_Book",
                column: "Fluent_Publisher_Id",
                principalTable: "Fluent_Publisher",
                principalColumn: "Publicher_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_Book_Fluent_Publisher_Fluent_Publisher_Id",
                table: "Fluent_Book");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_Book_Fluent_Publisher_Id",
                table: "Fluent_Book");

            migrationBuilder.DropColumn(
                name: "Fluent_Publisher_Id",
                table: "Fluent_Book");
        }
    }
}
