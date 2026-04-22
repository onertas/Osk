using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OskApi.Data;
using OskApi.ServiceRegisration;
using OskApi.Shared.Converters;
using OskApi.Shared.Mapping;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.WithOrigins("https://localhost:4200", "http://localhost:4200", "https://osk.onertas.com") // React uygulamanızın adresi
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials(); // Cookie'lerin gönderilmesine izin ver
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Cloudflare Tünel için en güvenlisi
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Cookie'lerin gönderilmesine izin ver
    });
});


//automapper
// Register AutoMapper profiles by specifying the assembly that contains them.
// Use the assembly of MappingProfile so the AddAutoMapper overload is resolved correctly.
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// 🔥 Tüm servis kayıtlarını tek satırda aldık
builder.Services.AddProjectServices(configuration);

// JWT
var jwtSettings = configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["accessToken"];
            return Task.CompletedTask;
        }

    };
});






builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.JsonSerializerOptions.Converters.Add(new TrimmingStringJsonConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();



//seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.SeedAsync(services);
        // İsteğe bağlı: Console.WriteLine("Veritabanı başarıyla hazırlandı.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı migration veya seed işlemi sırasında hata oluştu.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔁 Cloudflare / Reverse Proxy arkasında çalışırken X-Forwarded-Proto'yu oku
// Bu olmadan Secure=true cookie'ler HTTP üzerinden çalışmaz
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




app.Run();
