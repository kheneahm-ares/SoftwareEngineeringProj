using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodingBlogDemo2.Data.Migrations
{
    public partial class UpdatePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Posts",
                newName: "CourseId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Courses",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Posts",
                newName: "ClassId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Courses",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
