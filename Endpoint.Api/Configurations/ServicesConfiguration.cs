using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Infrastructure.Persistence;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Authentication.Commands;
using Application.Services.bases.Queries;
using Application.Services.Authentication.Queries;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Application.Services.Person.Commands;
using Application.Services.User.Commands;
using Application.AdminServices.User.Queries;
using Application.Services.bases.Commands;
using Application.Services.Question.Quesries;
using Application.Services.Question.Commands;
using Application.Services.Process.Quesries;
using Application.Services.Process.Queries;
using Application.Services.Process.Commands;
using Application.Services.Research.Queries;
using Application.Services.Research.Commands;
using Application.Services.Editor.Queries;
using Application.Services.Editor.Commands;
using Application.Services.Archive.Queries;
using Application.Services.Session.Queries;
using Application.Services.Session.Commands;
using Application.Services.User.Queries;
using Application.Services.Archive.Commands;
using Application.Services.Report.Queries;

namespace EndPoint.Api.Configurations
{
	public static class ServicesConfiguration
	{
		public static void Config(this IServiceCollection services, IConfiguration configuration)
		{
			#region database

			services.AddDbContext<DatabaseContext>
				(options => options.UseSqlServer(configuration.GetConnectionString("sql")), ServiceLifetime.Scoped);

			#endregion database

			#region Authentication

			services.AddAuthentication(Options =>
			{
				Options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
				Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(configureOptions =>
			{
				configureOptions.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidIssuer = configuration["jwtConfig:issuer"],
					ValidAudience = configuration["jwtConfig:audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtConfig:Key"])),
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
				};
				configureOptions.SaveToken = true;
				configureOptions.Events = new JwtBearerEvents
				{
					//OnTokenValidated = context =>
					//{
					//    var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();
					//    return tokenValidatorService.ExecuteAsync(context);
					//}
				};
			});

			#endregion Authentication

			#region Localization

			//services.AddLocalization(options => options.SetResources());

			#endregion Localization

			#region cuncurrent request
			//    services.AddMemoryCache();
			//    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
			//    services.AddInMemoryRateLimiting();
			//    services.Configure<IpRateLimitOptions>(options =>
			//    {
			//        options.EnableEndpointRateLimiting = true;
			//        options.StackBlockedRequests = false;
			//        options.HttpStatusCode = 429;
			//        options.ClientIdHeader = "Client-Id";
			//        options.GeneralRules = new List<RateLimitRule>
			//{
			//    new RateLimitRule
			//    {
			//        Endpoint = "post:/api/*",
			//        Period = "1s",
			//        Limit = 1
			//    }
			//};
			//    });



			//services.AddRateLimiter(options =>
			//{
			//    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
			//        RateLimitPartition.GetFixedWindowLimiter(
			//            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
			//            factory: partition => new FixedWindowRateLimiterOptions
			//            {
			//                AutoReplenishment = true,
			//                PermitLimit = 10,
			//                QueueLimit = 0,
			//                Window = TimeSpan.FromMinutes(1)
			//            }));
			//});
			#endregion

			#region Services

			#region DotNet
			services.AddSignalR();
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});
			services.Configure<FormOptions>(options =>
			{
				options.MultipartBodyLengthLimit = 100000000;
			});
			#endregion

			#region base
			//services.AddZarinpal(options =>
			//{
			//    options.MerchantId = "00da7f82-573d-4567-95af-b851abe491d6";
			//    options.ZarinpalMode = ZarinpalMode.Original;
			//});
			services.AddScoped<IDatabaseContext, DatabaseContext>();
			services.AddScoped<IGetProvincesService, GetProvincesService>();
			services.AddScoped<IGetCountriesService, GetCountriesService>();
			services.AddScoped<IGetJobCategoriesService, GetJobCategoriesService>();
			services.AddScoped<IGetLanguagesService, GetLanguagesService>();
			services.AddScoped<IGetMethodOfRecieveQuestionsService, GetMethodOfRecieveQuestionsService>();
			services.AddScoped<IGetQuestionCategoriesService, GetQuestionCategoriesService>();
			services.AddScoped<IAddCountryService,AddCountryService>();
			services.AddScoped<IAddJobCategoryService,AddJobCategoryService>();
			services.AddScoped<IAddLanguageService,AddLanguageService>();
			services.AddScoped<IAddMethodOfReceiveQuestionService, AddMethodOfReceiveQuestionService>();
			services.AddScoped<IAddProvinceService, AddProvinceService>();
			services.AddScoped<IAddQuestionCategoryService, AddQuestionCategoryService>();
			services.AddScoped<IUpdateCountryService, UpdateCountryService>();
			services.AddScoped<IUpdateJobCategoryService, UpdateJobCategoryService>();
			services.AddScoped<IUpdateQuestionCategoryService, UpdateQuestionCategoryService>();
			services.AddScoped<IUpdateLanguageService, UpdateLanguageService>();
			services.AddScoped<IUpdateMethodOfReceiveQuestionService, UpdateMethodOfReceiveQuestionService>();
			services.AddScoped<IUpdateProvinceService, UpdateProvinceService>();
			services.AddScoped<IDeleteCountryService, DeleteCountryService>();
			services.AddScoped<IDeleteJobCategoryService, DeleteJobCategoryService>();
			services.AddScoped<IDeleteQuestionCategoryService, DeleteQuestionCategoryService>();
			services.AddScoped<IDeleteLanguageService, DeleteLanguageService>();
			services.AddScoped<IDeleteMethodOfReceiveQuestionService , DeleteMethodOfReceiveQuestionService>();
			services.AddScoped<IDeleteProvinceService , DeleteProvinceService>();
			services.AddScoped<IGetOneCountryService, GetOneCountryService>();
			services.AddScoped<IGetOneJobCategoryService, GetOneJobCategoryService>();
			services.AddScoped<IGetOneLanguageService, GetOneLanguageService>();
			services.AddScoped<IGetOneMethodOfRecieveQuestionService, GetOneMethodOfRecieveQuestionService>();
			services.AddScoped<IGetOneProvinceService, GetOneProvinceService>();
			services.AddScoped<IGetUsersByRolesService,GetUsersByRolesService>();
			#endregion

			#region authentications
			services.AddScoped<IBuildToken, BuildToken>();
			services.AddScoped<ITokenContents, TokenContents>();
			services.AddScoped<ILoginService, LoginService>();
			services.AddScoped<IGetMenuAccessService, GetMenuAccessService>();
			services.AddScoped<IGetAccessService, GetAccessService>();
			services.AddScoped<IGetOneAccessService, GetOneAccessService>();
			#endregion

			#region users
			services.AddScoped<IAddUserService, AddUserService>();
			services.AddScoped<IDeleteUserService, DeleteUserService>();
			services.AddScoped<IUpdateUserAccessService, UpdateUserAccessService>();
			services.AddScoped<IGetUsersService, GetUsersService>();
			services.AddScoped<IGetUserAccessService, GetUserAccessService>();
			services.AddScoped<IResetPasswordService,ResetPasswordService>();
			services.AddScoped<IGetOneUserService, GetOneUserService>();
			services.AddScoped<IUpdateUserService, UpdateUserService>();
			#endregion

			#region Questions
			services.AddScoped<IGetQuestionsService, GetQuestionsService>();
			services.AddScoped<IGetOneQuestionService, GetOneQuestionService>();
			services.AddScoped<IAddQuestionService, AddQuestionService>();
			services.AddScoped<IEditQuestionService, EditQuestionService>();
			services.AddScoped<IDeleteQuestionService, DeleteQuestionService>();
			services.AddScoped<IDeleteQuestionFileService, DeleteQuestionFileService>();
			services.AddScoped<IReferenceToProcessorService, ReferenceToProcessorService>();
			services.AddScoped<IGetPopupQuestionService, GetPopupQuestionService>();
			#endregion

			#region Process
			services.AddScoped<IGetProcessQuestionsService, GetProcessQuestionsService>();
			services.AddScoped<IGetProcessCyclesService, GetProcessCyclesService>();
			services.AddScoped<ISearchQuestionService, SearchQuestionService>();
			services.AddScoped<IAddProcessQuestionService, AddProcessQuestionService>();
			services.AddScoped<IAcceptOrRejectProcessService, AcceptOrRejectProcessService>();
			#endregion

			#region Research
			services.AddScoped<IGetResearchQuestionsService, GetResearchQuestionsService>();
			services.AddScoped<IGetResearchCyclesService, GetResearchCyclesService>();
			services.AddScoped<IAddResearchQuestionService, AddResearchQuestionService>();
			services.AddScoped<IAcceptOrRejectResearchService, AcceptOrRejectResearchService>();
			#endregion

			#region Editor
			services.AddScoped<IGetEditorQuestionsService, GetEditorQuestionsService>();
			services.AddScoped<IGetEditorCyclesService, GetEditorCyclesService>();
			services.AddScoped<IAddEditorQuestionService, AddEditorQuestionService>();
			services.AddScoped<IAcceptOrRejectEditorService, AcceptOrRejectEditorService>();
			#endregion

			#region archive
			services.AddScoped<IGetArchiveQuestionService,GetArchiveQuestionService>();
			services.AddScoped<IGetQuestionStepsService,GetQuestionStepsService>();
			services.AddScoped<IGetQuestionResultService, GetQuestionResultService>();
			services.AddScoped<IDeleteArchiveQuestionService, DeleteArchiveQuestionService>();
			#endregion

			#region sessions
			services.AddScoped<IGetSessionsService,GetSessionsService>();
			services.AddScoped<IGetOneSessionService,GetOneSessionService>();
			services.AddScoped<IAddSessionService,AddSessionService>();
			services.AddScoped<IUpdateSessionService, UpdateSessionService>();
			services.AddScoped<IDeleteSessionService, DeleteSessionService>();
			#endregion

			#region reports
			services.AddScoped<IGetUserQueryService, GetUserQueryService>();
			#endregion

			#endregion services

			#region automapper
			//services.AddAutoMapper(typeof(Program));
			#endregion

			#region defaults

			var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
			services.AddCors(options =>
			{

				options.AddPolicy(name: MyAllowSpecificOrigins,
								  builder =>
								  {
									  builder.SetIsOriginAllowed(s => s.Contains("istifta.amoozeshbeseh.ir") || s.Contains("localhost") || s.Contains("185.142.157.171"));
									  //builder.AllowAnyOrigin();
									  //builder.WithOrigins("http://form.amoozeshbeseh.ir");
									  builder.AllowAnyHeader();
									  builder.AllowAnyMethod();
									  builder.AllowCredentials();
									  //builder.AllowAnyOrigin();
									  //builder.WithExposedHeaders("*");
								  });
			});



			services.AddControllers().AddFluentValidation(fv =>
			{
				fv.DisableDataAnnotationsValidation = true;
			});

			//.AddDataAnnotationsLocalization(options =>
			//{
			//    options.DataAnnotationLocalizerProvider = (type, factory) =>
			//    {
			//        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
			//        var localizer = factory.Create("SharedResource", assemblyName.Name);
			//        Localizer.CustomLocalizer = localizer;
			//        return localizer;
			//    };
			//});
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			#endregion defaults

			#region Swagger

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Istifta",
					Version = "v1",
					Contact = new OpenApiContact
					{
						Email = "m.h.adib1368@gmail.com",
						Name = "M-h-adib"
					}
				});

				//c.OrderActionsBy(a => a.ParameterDescriptions.Count.ToString());
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
					{
						  new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new string[] {}
					}
			});

				///project properties => Build => check documentation file
				//var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				//var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				//c.IncludeXmlComments(xmlPath);

			});

			#endregion Swagger
		}
	}
}
