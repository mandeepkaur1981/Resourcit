using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResourcitAPI.Migrations
{
    public partial class adddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "Budget", "Description", "Enddate", "ShortDescription", "Startdate" },
                values: new object[,]
                {
                    { 1, 45000m, "C# Project", new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "C#", new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 900000m, "ASP.NET Web Project", new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Web", new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 145000m, "React Project", new DateTime(2022, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "React", new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "ResourceId", "Firstname", "Hiredate", "Lastname", "Middlename", "Termdate" },
                values: new object[,]
                {
                    { 1, "Mandeep", new DateTime(2017, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kaur", "", null },
                    { 2, "Harpreet", new DateTime(2018, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Singh", "", new DateTime(2019, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Manpreet", new DateTime(2020, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Singh", "Pal", null },
                    { 4, "Neharika", new DateTime(2021, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rambhatla", "", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 4);
        }
    }
}
