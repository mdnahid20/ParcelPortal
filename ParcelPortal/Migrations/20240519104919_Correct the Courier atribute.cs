using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPortal.Migrations
{
    /// <inheritdoc />
    public partial class CorrecttheCourieratribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecevierPhone",
                table: "Courier",
                newName: "ReceiverPhone");

            migrationBuilder.RenameColumn(
                name: "RecevierName",
                table: "Courier",
                newName: "ReceiverName");

            migrationBuilder.RenameColumn(
                name: "RecevierEmail",
                table: "Courier",
                newName: "ReceiverEmail");

            migrationBuilder.RenameColumn(
                name: "RecevierAddress",
                table: "Courier",
                newName: "ReceiverAddress");

            migrationBuilder.AlterColumn<string>(
                name: "ConsignmentNumber",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverPhone",
                table: "Courier",
                newName: "RecevierPhone");

            migrationBuilder.RenameColumn(
                name: "ReceiverName",
                table: "Courier",
                newName: "RecevierName");

            migrationBuilder.RenameColumn(
                name: "ReceiverEmail",
                table: "Courier",
                newName: "RecevierEmail");

            migrationBuilder.RenameColumn(
                name: "ReceiverAddress",
                table: "Courier",
                newName: "RecevierAddress");

            migrationBuilder.AlterColumn<string>(
                name: "ConsignmentNumber",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
