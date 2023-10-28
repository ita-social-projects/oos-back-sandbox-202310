using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutOfSchool.Migrations.Data.Migrations.OutOfSchoolMigrations;

public partial class AchievementsTypesAddTypes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Переможці міжнародних та всеукраїнських спортивних змагань (індивідуальних та командних)', 'ua')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Призери та учасники міжнародних, всеукраїнських та призери регіональних конкурсів і виставок наукових, технічних, дослідницьких, інноваційних, ІТ проектів', 'ua')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Реципієнти міжнародних грантів', 'ua')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Призери міжнародних культурних конкурсів та фестивалів', 'ua')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Соціально активні категорії учнів', 'ua')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Цифрові інструменти Google для закладів вищої та фахової передвищої освіти', 'ua')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Переможці та учасники олімпіад міжнародного та всеукраїнського рівнів', 'ua')");

        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Winners of international and all-Ukrainian sports competitions (individual and team)', 'en')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Winners and participants of international, all-Ukrainian and regional contests and exhibitions of scientific, technical, research, innovation, IT projects', 'en')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Recipients of international grants', 'en')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Winners of international cultural competitions and festivals', 'en')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Socially active categories of students', 'en')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Googles digital tools for institutions of higher and professional pre-higher education', 'en')");
        migrationBuilder.Sql("INSERT INTO ChildAchievementTypes (`Type`, `Localization`) VALUES ('Winners and participants of olympiads at the international and all-Ukrainian levels', 'en')");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
