using FlyersSoft.Model;
using FlyersSoft.Services.Implementation;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtOptions = builder.Configuration
    .GetSection("JwtOptions")
    .Get<JwtOptions>();

builder.Services.AddSingleton(jwtOptions);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();
builder.Services.AddScoped<ITopupService, TopupService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddDbContext<MyDBContext>( options=>
{
    options.UseInMemoryDatabase("MyDB");
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        //convert the string signing key to byte array
        byte[] signingKeyBytes = Encoding.UTF8
            .GetBytes(jwtOptions.SigningKey);

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/tokens/connect", (HttpContext ctx, JwtOptions jwtOptions, IUserService userService
    )
    => TokenEndpoint.Connect(ctx, jwtOptions, userService));

app.Run();
