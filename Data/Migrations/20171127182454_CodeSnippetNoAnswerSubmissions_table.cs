using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CodingBlogDemo2.Data.Migrations
{
    public partial class CodeSnippetNoAnswerSubmissions_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeSnippet",
                table: "CodeSnippet");

            migrationBuilder.RenameTable(
                name: "CodeSnippet",
                newName: "CodeSnippets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeSnippets",
                table: "CodeSnippets",
                column: "CodeSnippetId");

            migrationBuilder.CreateTable(
                name: "CodeSnippetNoAnswerSubmissions",
                columns: table => new
                {
                    CodeSnippetNoAnswerSubmissionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssignmentId = table.Column<int>(nullable: false),
                    IsCorrect = table.Column<bool>(nullable: false),
                    UserCode = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(nullable: true),
                    WhenCreated = table.Column<DateTime>(nullable: false),
                    WhenEdited = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnippetNoAnswerSubmissions", x => x.CodeSnippetNoAnswerSubmissionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeSnippetNoAnswerSubmissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeSnippets",
                table: "CodeSnippets");

            migrationBuilder.RenameTable(
                name: "CodeSnippets",
                newName: "CodeSnippet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeSnippet",
                table: "CodeSnippet",
                column: "CodeSnippetId");
        }
    }
}
