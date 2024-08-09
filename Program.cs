using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using App.Interface;
using App.CategoryRepository;
using App.ProductRepository;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// builder.Services.AddDbContext<TodoContext>(opt =>
//     opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<AppDbContext>(x =>
{
    string connectionString = builder.Configuration.GetConnectionString("Connecdb");
    x.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<User, IdentityRole>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = true;
    o.Password.RequireNonAlphanumeric = true;
    o.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme =
    o.DefaultChallengeScheme =
    o.DefaultForbidScheme =
    o.DefaultScheme =
    o.DefaultSignInScheme =
    o.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();