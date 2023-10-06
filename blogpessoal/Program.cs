using blogpessoal.Data;
using blogpessoal.model;
using blogpessoal.Security;
using blogpessoal.Security.implements;
using blogpessoal.Service;
using blogpessoal.Service.Implements;
using blogpessoal.validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace blogpessoal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);//Cria a variável que receberá uma nova aplicação WEB, criada pelo Método CreateBuilder(), da Classe WebApplication

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });//Adiciona o Serviço Controllers, através do Método AddControllers(), responsável por gerenciar as Classes Controladoras da aplicação e os respectivos endpoints (rotas), responsáveis por acessar os Métodos de cada recurso da aplicação.

            // COnexão com o Banco de Dados

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // registrar a Validação da Entidades

            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>();
            builder.Services.AddTransient<IValidator<Tema>, TemaValidator>();
            builder.Services.AddTransient<IValidator<User>, UserValidator>();

            // Registrar as Classesde Serviço
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
            builder.Services.AddSwaggerGen();// Adiciona o Serviço EndpointsApiExplorer, através do Método AddEndpointsApiExplorer(), responsável por registrar os endpoints(rotas) da aplicação, permitindo que eles sejam expostos(acessíveis).

            //Adiciona o Serviço SwaggerGen, através do Método AddSwaggerGen(), responsável por gerar a documentação da API através do Swagger.



            //Configuração do CORS é um mecanismo de segurança usado pelos navegadores para permitir ou bloquear requisições de recursos entre diferentes origens (domínios).
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
            var app = builder.Build();// Cria uma variável chamada app, que receberá a aplicação WEB, criada pelo Método CreateBuilder(), inicializada com todas as configurações efetuadas acima, através do Método Build().

            // Criar o banco de dados e as tabelas

            using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // inicializa o CORS
            app.UseCors("MyPolicy");
           
            app.UseAuthentication();
           
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}