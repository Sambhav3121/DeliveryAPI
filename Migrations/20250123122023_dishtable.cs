using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sambackend.Migrations
{
    /// <inheritdoc />
    public partial class dishtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_storeTokens",
                table: "storeTokens");

            migrationBuilder.RenameTable(
                name: "storeTokens",
                newName: "StoreTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreTokens",
                table: "StoreTokens",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreTokens",
                table: "StoreTokens");

            migrationBuilder.RenameTable(
                name: "StoreTokens",
                newName: "storeTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_storeTokens",
                table: "storeTokens",
                column: "Id");
        }
    }
}
