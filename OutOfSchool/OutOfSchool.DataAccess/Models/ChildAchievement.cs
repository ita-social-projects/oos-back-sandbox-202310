using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Models;
public class ChildAchievement : IKeyedEntity<Guid>
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public DateTime Date { get; set; }

    public string Trainer { get; set; }

    public Guid ChildId { get; set; }

    public Child Child { get; set; }

    public Guid WorkshopId { get; set; }

    public Workshop Workshop { get; set; }

}
