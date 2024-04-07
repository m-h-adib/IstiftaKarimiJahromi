using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;

public class BaseEntity<IdType>
{
	public IdType Id { get; set; }

	public bool IsRemoved { get; set; } = false;
	public bool IsActive { get; set; } = true;
	public DateTime CreatedAt { get; set; } = DateTime.Now;
	public DateTime? UpdatedAt { get; set; }
	public DateTime? RemovedAt { get; set; }
}
