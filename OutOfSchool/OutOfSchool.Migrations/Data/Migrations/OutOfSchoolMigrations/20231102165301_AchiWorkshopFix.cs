using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutOfSchool.Migrations.Data.Migrations.OutOfSchoolMigrations
{
    public partial class AchiWorkshopFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildAchievements_Workshops_WorkshopId",
                table: "ChildAchievements");

            migrationBuilder.DropIndex(
                name: "IX_ChildAchievements_WorkshopId",
                table: "ChildAchievements");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "ChildAchievements"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkshopId",
                table: "ChildAchievements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChildAchievements_WorkshopId",
                table: "ChildAchievements",
                column: "WorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildAchievements_Workshops_WorkshopId",
                table: "ChildAchievements",
                column: "WorkshopId",
                principalTable: "Workshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }
    }
}
