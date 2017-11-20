using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodingBlogDemo2.Data.Migrations
{
    public partial class TimeStampInSelectedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WhenCreated",
                table: "MultipleChoiceSubmissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenEdited",
                table: "MultipleChoiceSubmissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenCreated",
                table: "MultipleChoices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenEdited",
                table: "MultipleChoices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenCreated",
                table: "Courses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenEdited",
                table: "Courses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenCreated",
                table: "CodeSnippetNoAnswers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenEdited",
                table: "CodeSnippetNoAnswers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenCreated",
                table: "CodeSnippet",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenEdited",
                table: "CodeSnippet",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "MultipleChoiceSubmissions");

            migrationBuilder.DropColumn(
                name: "WhenEdited",
                table: "MultipleChoiceSubmissions");

            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "MultipleChoices");

            migrationBuilder.DropColumn(
                name: "WhenEdited",
                table: "MultipleChoices");

            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "WhenEdited",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "CodeSnippetNoAnswers");

            migrationBuilder.DropColumn(
                name: "WhenEdited",
                table: "CodeSnippetNoAnswers");

            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "CodeSnippet");

            migrationBuilder.DropColumn(
                name: "WhenEdited",
                table: "CodeSnippet");
        }
    }
}
