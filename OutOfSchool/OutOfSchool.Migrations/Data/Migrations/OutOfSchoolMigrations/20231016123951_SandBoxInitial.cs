using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutOfSchool.Migrations.Data.Migrations.OutOfSchoolMigrations;

public partial class SandBoxInitial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AchievementChild");

        migrationBuilder.DropTable(
            name: "AchievementTeachers");

        migrationBuilder.DropTable(
            name: "AreaAdmins");

        migrationBuilder.DropTable(
            name: "Favorites");

        migrationBuilder.DropTable(
            name: "InstitutionAdmins");

        migrationBuilder.DropTable(
            name: "Ratings");

        migrationBuilder.DropTable(
            name: "RegionAdmins");

        migrationBuilder.DropTable(
            name: "Achievements");

        migrationBuilder.DropTable(
            name: "AchievementTypes");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
                name: "AchievementTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TitleEn = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementTypes", x => x.Id);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "AreaAdmins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CATOTTGId = table.Column<long>(type: "bigint", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaAdmins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AreaAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaAdmins_CATOTTGs_CATOTTGId",
                        column: x => x.CATOTTGId,
                        principalTable: "CATOTTGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaAdmins_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkshopId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "InstitutionAdmins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstitutionId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionAdmins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_InstitutionAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionAdmins_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    EntityId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Rate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "RegionAdmins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CATOTTGId = table.Column<long>(type: "bigint", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionAdmins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_RegionAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionAdmins_CATOTTGs_CATOTTGId",
                        column: x => x.CATOTTGId,
                        principalTable: "CATOTTGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionAdmins_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    AchievementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    WorkshopId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    AchievementDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Title = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievements_AchievementTypes_AchievementTypeId",
                        column: x => x.AchievementTypeId,
                        principalTable: "AchievementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Achievements_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "AchievementChild",
                columns: table => new
                {
                    AchievementsId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    ChildrenId = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementChild", x => new { x.AchievementsId, x.ChildrenId });
                    table.ForeignKey(
                        name: "FK_AchievementChild_Achievements_AchievementsId",
                        column: x => x.AchievementsId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementChild_Children_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                name: "AchievementTeachers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AchievementId = table.Column<Guid>(type: "binary(16)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AchievementTeachers_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.InsertData(
            table: "AchievementTypes",
            columns: new[] { "Id", "IsDeleted", "Title", "TitleEn" },
            values: new object[,]
            {
                { 1L, false, "Переможці міжнародних та всеукраїнських спортивних змагань (індивідуальних та командних)", "Winners of international and all-Ukrainian sports competitions (individual and team)" },
                { 2L, false, "Призери та учасники міжнародних, всеукраїнських та призери регіональних конкурсів і виставок наукових, технічних, дослідницьких, інноваційних, ІТ проектів", "Winners and participants of international, all-Ukrainian and regional contests and exhibitions of scientific, technical, research, innovation, IT projects" },
                { 3L, false, "Реципієнти міжнародних грантів", "Recipients of international grants" },
                { 4L, false, "Призери міжнародних культурних конкурсів та фестивалів", "Winners of international cultural competitions and festivals" },
                { 5L, false, "Соціально активні категорії учнів", "Socially active categories of students" },
                { 6L, false, "Цифрові інструменти Google для закладів вищої та фахової передвищої освіти", "Google digital tools for institutions of higher and professional pre-higher education" },
                { 7L, false, "Переможці та учасники олімпіад міжнародного та всеукраїнського рівнів", "Winners and participants of olympiads at the international and all-Ukrainian levels" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_AchievementChild_ChildrenId",
            table: "AchievementChild",
            column: "ChildrenId");

        migrationBuilder.CreateIndex(
            name: "IX_Achievements_AchievementTypeId",
            table: "Achievements",
            column: "AchievementTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_Achievements_IsDeleted",
            table: "Achievements",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Achievements_WorkshopId",
            table: "Achievements",
            column: "WorkshopId");

        migrationBuilder.CreateIndex(
            name: "IX_AchievementTeachers_AchievementId",
            table: "AchievementTeachers",
            column: "AchievementId");

        migrationBuilder.CreateIndex(
            name: "IX_AchievementTeachers_IsDeleted",
            table: "AchievementTeachers",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_AchievementTypes_IsDeleted",
            table: "AchievementTypes",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_AreaAdmins_CATOTTGId",
            table: "AreaAdmins",
            column: "CATOTTGId");

        migrationBuilder.CreateIndex(
            name: "IX_AreaAdmins_InstitutionId",
            table: "AreaAdmins",
            column: "InstitutionId");

        migrationBuilder.CreateIndex(
            name: "IX_AreaAdmins_IsDeleted",
            table: "AreaAdmins",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Favorites_IsDeleted",
            table: "Favorites",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Favorites_UserId",
            table: "Favorites",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Favorites_WorkshopId",
            table: "Favorites",
            column: "WorkshopId");

        migrationBuilder.CreateIndex(
            name: "IX_InstitutionAdmins_InstitutionId",
            table: "InstitutionAdmins",
            column: "InstitutionId");

        migrationBuilder.CreateIndex(
            name: "IX_InstitutionAdmins_IsDeleted",
            table: "InstitutionAdmins",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Ratings_EntityId",
            table: "Ratings",
            column: "EntityId");

        migrationBuilder.CreateIndex(
            name: "IX_Ratings_IsDeleted",
            table: "Ratings",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Ratings_ParentId",
            table: "Ratings",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_RegionAdmins_CATOTTGId",
            table: "RegionAdmins",
            column: "CATOTTGId");

        migrationBuilder.CreateIndex(
            name: "IX_RegionAdmins_InstitutionId",
            table: "RegionAdmins",
            column: "InstitutionId");

        migrationBuilder.CreateIndex(
            name: "IX_RegionAdmins_IsDeleted",
            table: "RegionAdmins",
            column: "IsDeleted");
    }
}