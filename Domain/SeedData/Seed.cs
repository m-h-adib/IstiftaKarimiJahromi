using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Domain.Entities;

namespace Domain.SeedData;

public static class Seed
{
	public static void SetSeedData(this ModelBuilder builder)
	{

		
		//foreach (var role in Enum.GetValues(typeof(ProvinceType)))
		//{
		//	builder.Entity<BaseRole>().HasData(new BaseRole
		//	{
		//		Id = ((int)role),
		//		Name = role.ToString()
		//	});
		//}



		////
		//builder.Entity<Transaction>()
		//	 .HasOne(p => p.Source)
		//	 .WithMany(p => p.Sources)
		//	 .HasForeignKey(p => p.SourceId)
		//	 .OnDelete(DeleteBehavior.NoAction);



		//prevent duplicate value by two column bookid and userid
		//builder.Entity<BookRead>()
		//	.HasIndex(p => new { p.BookId, p.UserId })
		//	.IsUnique();



		//builder.Entity<PhysicalBookCode>()
		//   .HasIndex(p => p.Code)
		//   .IsUnique();

	}
}
