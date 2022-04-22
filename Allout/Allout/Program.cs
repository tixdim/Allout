using Allout.DataAccess.Context;
using Allout.DataAccess.Core.Interfaces.DBContext;
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


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// builder.Services.AddAutoMapper(typeof(MicroserviceProfile), typeof(BusinessLogicProfile));
builder.Services.AddDbContext<IContext, AlloutContext>(b => b.UseSqlite("Data Source=usersdata.db; Foreign Keys=True", o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

// Add Services
// builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IUserFriendsAndContactsService, UserFriendsAndContactsService>();
// builder.Services.AddScoped<IFriendInvitationService, FriendInvitationService>();
// builder.Services.AddScoped<IMedalService, MedalService>();
// builder.Services.AddScoped<IPostService, UserMultimediaPostService>();

builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(allowSpecificOrigins);

app.UseAuthorization();

using var scope = app.Services.CreateScope();
// var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
// mapper.ConfigurationProvider.AssertConfigurationIsValid();

var dbContext = scope.ServiceProvider.GetRequiredService<AlloutContext>();
dbContext.Database.Migrate();

app.MapControllers();

app.Run();
