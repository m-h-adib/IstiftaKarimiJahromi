using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class AdminSectionAccess:BaseEntity<long>
{
	#region navigations
	public long AdminSectionId { get; set; }
	public AdminSection? AdminSection { get; set; }
	public long UserId { get; set; }
	public User? User { get; set; }
	#endregion
}
