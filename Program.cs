using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Blog.Data;
using blogapi.Controller;
using blogapi.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureAuthorization(builder);
ConfigureService(builder);
ConfigureMVC(builder);

var app = builder.Build();
LoadConfiguration(app);
//para forcar um https
app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();



void LoadConfiguration(WebApplication app)
{
    Configuration.JwtToken = app.Configuration.GetValue<string>("JwtToken");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    //podemos usar o Bind para ler todos os dados de uma seção no json, inves de passar um por um
    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}
void ConfigureAuthorization(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtToken);
        builder.Services.AddAuthentication(x =>
        {
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
        }).AddJwtBearer(x=> {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };    
        });
}
void ConfigureService(WebApplicationBuilder builder)

{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCompression(options => {
        options.Providers.Add<GzipCompressionProvider>();
    });
    builder.Services.Configure<GzipCompressionProviderOptions>(options => {
        options.Level = CompressionLevel.Optimal;
    });
    builder.Services.AddDbContext<BlogDataContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
    builder.Services.AddControllers();
}

void ConfigureMVC(WebApplicationBuilder builder)
{
    builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(
        options => {
            options.SuppressModelStateInvalidFilter = true;
            }).AddJsonOptions(x => {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
}