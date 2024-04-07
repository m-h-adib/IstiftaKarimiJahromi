using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Country:BaseEntity<long>
{
    public string? Title { get; set; }
    public int? Order { get; set; }

    #region navigations
    public ICollection<Province>? Provinces { get; set; }
	public ICollection<Question>? Questions { get; set; }
	#endregion
}
