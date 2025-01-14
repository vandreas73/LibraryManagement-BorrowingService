using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BorrowingService.Migrations
{
    /// <inheritdoc />
    public partial class nullableReturnedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "Borrows");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReturnedAt",
                table: "Borrows",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReturnedAt",
                table: "Borrows",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "Borrows",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
