using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CodingBlogDemo2.Data.Migrations
{
    public partial class ChanginPost_AddingTypeAndMcModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "PostCategory",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoices",
                columns: table => new
                {
                    MultipleChoiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    A = table.Column<string>(nullable: true),
                    Answer = table.Column<char>(nullable: false),
                    B = table.Column<string>(nullable: true),
                    C = table.Column<string>(nullable: true),
                    D = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoices", x => x.MultipleChoiceId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "MultipleChoices");

            migrationBuilder.DropColumn(
                name: "PostCategory",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Posts",
                nullable: true);
        }
    }
}
