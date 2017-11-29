using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodingBlogDemo2.Data.Migrations
{
    public partial class CSNASubmissionRevision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserCode",
                table: "CodeSnippetNoAnswerSubmissions",
                newName: "UserAnswer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserAnswer",
                table: "CodeSnippetNoAnswerSubmissions",
                newName: "UserCode");
        }
    }
}
