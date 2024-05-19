using Application.Interfaces.Contexts;
using Domain.Entities;
using Domain.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence;

public class DatabaseContext: DbContext, IDatabaseContext
{
	#region ctors

	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
	{

	}

	#endregion ctors
	#region configurations

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var assembly = Assembly.Load("Domain");
		modelBuilder.ApplyConfigurationsFromAssembly(assembly);
	    modelBuilder.SetSeedData();
		base.OnModelCreating(modelBuilder);
	}

	#endregion configurations

	#region dbSets
	public DbSet<User> Users { get; set; }
    public DbSet<AdminSection> AdminSections { get; set; }
    public DbSet<AdminSectionAccess> AdminSectionAccesses { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Editor> Editors { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
	public DbSet<Language> Languages { get; set; }
	public DbSet<Province> Provinces { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<QuestionAttach> QuestionAttaches { get; set; }
	public DbSet<QuestionCategory> QuestionCategories { get; set; }
	public DbSet<QuestionCycle> QuestionCycles { get; set; }
    public DbSet<MethodOfReceiveQuestion> MethodOfReceiveQuestions { get; set; }
    public DbSet<Research> Researchs { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionUser> SessionUsers { get; set; }
    public DbSet<PreProccess> PreProccesses { get; set; }
    #endregion dbSets
}
