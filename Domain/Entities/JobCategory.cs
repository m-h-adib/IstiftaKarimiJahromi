using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class JobCategory:BaseEntity<long>
{
    public string? Title { get; set; }
    public int? Order { get; set; }

    #region navigations
    public ICollection<Question>? Questions { get; set; }
    #endregion
}
