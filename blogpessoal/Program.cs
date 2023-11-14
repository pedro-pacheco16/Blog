using blogpessoal.Configuration;
using blogpessoal.Data;
using blogpessoal.model;
using blogpessoal.Security;
using blogpessoal.Security.implements;
using blogpessoal.Service;
using blogpessoal.Service.Implements;
using blogpessoal.validator;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace blogpessoal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);//Cria a vari�vel que receber� uma nova aplica��o WEB, criada pelo M�todo CreateBuilder(), da Classe WebApplication

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });//Adiciona o Servi�o Controllers, atrav�s do M�todo AddControllers(), respons�vel por gerenciar as Classes Controladoras da aplica��o e os respectivos endpoints (rotas), respons�veis por acessar os M�todos de cada recurso da aplica��o.

            // COnex�o com o Banco de Dados
            if (builder.Configuration["Enviroment:Start"] == "PROD")
            {
                builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");

                var connectionString = builder.Configuration.GetConnectionString("ProdConnection");

                builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));


            }
            else
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            }
            // registrar a Valida��o da Entidades

            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>();
            builder.Services.AddTransient<IValidator<Tema>, TemaValidator>();
            builder.Services.AddTransient<IValidator<User>, UserValidator>();

            // Registrar as Classesde Servi�o
            builder.Services.AddScoped<IPostagemService, PostagemService>();
            builder.Services.AddScoped<ITemaService, TemaService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(Settings.Secret);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)


                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Projeto Blog Pessoal",
                    Description = "Projeto Blog Pessoal",
                    Contact = new OpenApiContact
                    {
                        Name = "Pedro Pacheco",
                        Email = "pedroaugustopacheco16@gmail.com",
                        Url = new Uri("https://github.com/pedro-pacheco16")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Github",
                        Url = new Uri("https://github.com/pedro-pacheco16")
                    }
                });

                options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Digite um Token V�lido",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // adicionar a indica��o de endpoint protegido
                options.OperationFilter<AuthResponsesOperationFilter>();
            });

            //adicionar o fluent validation no swagger
            builder.Services.AddFluentValidationRulesToSwagger();

            //Configura��o do CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            var app = builder.Build();// Cria uma vari�vel chamada app, que receber� a aplica��o WEB, criada pelo M�todo CreateBuilder(), inicializada com todas as configura��es efetuadas acima, atrav�s do M�todo Build().

            // Criar o banco de dados e as tabelas

            using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            ///{
            app.UseSwagger();

            //Swagger Como P�gina Inicial na Nuvem

            if (app.Environment.IsProduction())
            {
                app.UseSwaggerUI(options =>
                {

                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog Pessoal - v1");
                    options.RoutePrefix = string.Empty;

                });

            }

            //}
            app.UseSwaggerUI();
            // inicializa o CORS
            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}