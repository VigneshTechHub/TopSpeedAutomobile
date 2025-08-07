using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopSpeed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatetOnToCreatedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatetOn",
                table: "VehicleType",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatetOn",
                table: "Post",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatetOn",
                table: "Brand",
                newName: "CreatedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "VehicleType",
                newName: "CreatetOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Post",
                newName: "CreatetOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Brand",
                newName: "CreatetOn");
        }
    }
}
