using System.Text;
using Blog.Data;
using blogapi.Controller;
using blogapi.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddControllers();

ConfigureAuthorization(builder);
ConfigureService(builder);

var app = builder.Build();
LoadConfiguration(app);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
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
    builder.Services.AddDbContext<BlogDataContext>();
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
    builder.Services.AddControllers();
}
