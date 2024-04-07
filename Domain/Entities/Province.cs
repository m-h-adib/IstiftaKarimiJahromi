using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Province:BaseEntity<long>
{
    public string? Title { get; set; }
    public int? Order { get; set; }

    #region navigations
    public long? CountryId { get; set; }
    public Country? Country { get; set; }
    public ICollection<Question>? Qusetions { get; set; }
    #endregion
}
