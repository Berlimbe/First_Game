using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoInimigoNaRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnemyClass",
                table: "Runs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EnemyCurrentHp",
                table: "Runs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnemyMaxHp",
                table: "Runs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EnemyName",
                table: "Runs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnemyClass",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "EnemyCurrentHp",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "EnemyMaxHp",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "EnemyName",
                table: "Runs");
        }
    }
}
