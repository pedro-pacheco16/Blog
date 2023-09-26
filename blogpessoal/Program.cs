using blogpessoal.Data;
using blogpessoal.model;
using blogpessoal.Service;
using blogpessoal.Service.Implements;
using blogpessoal.validator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace blogpessoal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);//Cria a vari�vel que receber� uma nova aplica��o WEB, criada pelo M�todo CreateBuilder(), da Classe WebApplication

            // Add services to the container.

            builder.Services.AddControllers();//Adiciona o Servi�o Controllers, atrav�s do M�todo AddControllers(), respons�vel por gerenciar as Classes Controladoras da aplica��o e os respectivos endpoints (rotas), respons�veis por acessar os M�todos de cada recurso da aplica��o.

            // COnex�o com o Banco de Dados

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // registrar a Valida��o da Entidades

            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>();

            // Registrar as Classesde Servi�o
            builder.Services.AddScoped<IPostagemService, PostagemService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();// Adiciona o Servi�o EndpointsApiExplorer, atrav�s do M�todo AddEndpointsApiExplorer(), respons�vel por registrar os endpoints(rotas) da aplica��o, permitindo que eles sejam expostos(acess�veis).

            //Adiciona o Servi�o SwaggerGen, atrav�s do M�todo AddSwaggerGen(), respons�vel por gerar a documenta��o da API atrav�s do Swagger.



            //Configura��o do CORS � um mecanismo de seguran�a usado pelos navegadores para permitir ou bloquear requisi��es de recursos entre diferentes origens (dom�nios).
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
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // inicializa o CORS
            app.UseCors("MyPolicy");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}