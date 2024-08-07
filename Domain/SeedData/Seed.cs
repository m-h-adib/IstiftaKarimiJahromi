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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNet.Identity;

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

		#region superadmin
		PasswordHasher hasher = new PasswordHasher();
		string pass = hasher.HashPassword("871107077");

		builder.Entity<User>()
			.HasData(new User
			{
				Id=1,
				Name = "محمدحسین ادیب",
				CreatedAt = DateTime.Now,
				IsRemoved = false,
				IsActive = true,
				Username = "m-h-adib",
				Password = pass,
				Role = RoleType.Admin
			});
		#endregion


		builder.Entity<QuestionCycle>()
			.HasOne(p=>p.FromUser)
			.WithMany(p=>p.QuestionCyclesFromUsers)
			.HasForeignKey(p=>p.FromUserId)
			.OnDelete(DeleteBehavior.NoAction);

        builder.Entity<QuestionCycle>()
            .HasOne(p => p.ToUser)
            .WithMany(p => p.QuestionCyclesToUsers)
            .HasForeignKey(p => p.ToUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
