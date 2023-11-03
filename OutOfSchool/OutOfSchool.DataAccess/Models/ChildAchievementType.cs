namespace OutOfSchool.Services.Models;
public class ChildAchievementType : IKeyedEntity<int>
{
    public int Id { get; set; }

    public string Type { get; set; }

    public string Localization { get; set; }
}
