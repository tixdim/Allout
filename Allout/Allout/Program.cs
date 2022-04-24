using Allout.BusinessLogic.AutoMapperProfile;
using Allout.BusinessLogic.Core.Interfaces;
using Allout.BusinessLogic.Services;
using Allout.DataAccess.Context;
using Allout.DataAccess.Core.Interfaces.DBContext;
using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var allowSpecificOrigins = "_allowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        builder =>
        {
            builder.WithHeaders(HeaderNames.ContentType, "ApiKey").AllowAnyOrigin();
        });
});


builder.Services.AddAutoMapper(typeof(BusinessLogicProfile));
builder.Services.AddDbContext<IContext, AlloutContext>(b => b.UseSqlite("Data Source=usersdata.db; Foreign Keys=True", o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<IBuyLotService, BuyLotService>();
builder.Services.AddScoped<IUserCommentService, UserCommentService>();
builder.Services.AddScoped<IUserStarService, UserStarService>();

builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(allowSpecificOrigins);

app.UseAuthorization();

using var scope = app.Services.CreateScope();

var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

var dbContext = scope.ServiceProvider.GetRequiredService<AlloutContext>();
dbContext.Database.Migrate();

app.MapControllers();

app.Run();
