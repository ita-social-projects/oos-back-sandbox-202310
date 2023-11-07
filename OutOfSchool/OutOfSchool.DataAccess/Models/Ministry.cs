using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Models;
public class Ministry : IKeyedEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<MinistryAdmin> MinistryAdmins { get; set; }
}
