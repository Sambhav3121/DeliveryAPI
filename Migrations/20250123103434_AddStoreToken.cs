using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace sambackend.Migrations
{
    /// <inheritdoc />
 public partial class AddStoreToken : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Add a new UUID column for 'Id'
        migrationBuilder.AddColumn<Guid>(
            name: "NewId",
            table: "Users",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()");  // You can use a random UUID generation

        // Optionally, you can use a custom UUID generation logic if you have existing data to migrate.

        // Create the storeTokens table
        migrationBuilder.CreateTable(
            name: "storeTokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                Token = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_storeTokens", x => x.Id);
            });

        // Drop the old 'Id' column (integer)
        migrationBuilder.DropColumn(
            name: "Id",
            table: "Users");

        // Rename the 'NewId' column to 'Id'
        migrationBuilder.RenameColumn(
            name: "NewId",
            table: "Users",
            newName: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Revert the changes by adding back the old column and dropping the new column
        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "Users",
            type: "integer",
            nullable: false,
            defaultValue: 0);  // Add the default value if needed

        migrationBuilder.DropColumn(
            name: "Id",
            table: "Users");

        migrationBuilder.RenameColumn(
            name: "NewId",
            table: "Users",
            newName: "Id");
    }
}
}