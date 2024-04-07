using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts;

public interface IDatabaseContext
{
	#region dbsets
	DbSet<User> Users { get; set; }
	DbSet<AdminSection> AdminSections { get; set; }
	DbSet<AdminSectionAccess> AdminSectionAccesses { get; set; }
	DbSet<Country> Countries { get; set; }
	DbSet<Editor> Editors { get; set; }
	DbSet<JobCategory> JobCategories { get; set; }
	DbSet<Language> Languages { get; set; }
	DbSet<Proccess> Proccesses { get; set; }
	DbSet<Province> Provinces { get; set; }
	DbSet<Question> Questions { get; set; }
	DbSet<QuestionAttach> QuestionAttaches { get; set; }
	DbSet<QuestionCategory> QuestionCategories { get; set; }
	DbSet<QuestionCycle> QuestionCycles { get; set; }
	DbSet<MethodOfReceiveQuestion> MethodOfReceiveQuestions { get; set; }
	DbSet<Research> Researchs { get; set; }
	DbSet<Session> Sessions { get; set; }
	DbSet<SessionUser> SessionUsers { get; set; }
	#endregion
	#region methods

	int SaveChanges(bool acceptAllChangesOnSuccess);

	int SaveChanges();

	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

	DatabaseFacade Database { get; }

	#endregion methods
}
