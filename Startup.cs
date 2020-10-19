using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Configurando a aplicação para forncecer os serviços

            //Cria um serviço transitório (será gerada uma nova instância sempre que esse serviço for chamado, em cada método)
            //services.AddTransient<ICatalogo, Catalogo>();
            //services.AddTransient<IRelatorio, Relatorio>();

            //Desta forma, só será gerada uma instância dos serviços para cada requisição
            services.AddScoped<ICatalogo, Catalogo>();
            services.AddScoped<IRelatorio, Relatorio>();

            //Com o singleton teremos uma instância única a cada execução da aplicação
            //var catalogo = new Catalogo();
            //services.AddSingleton<ICatalogo>(catalogo);
            //services.AddSingleton<IRelatorio>(new Relatorio(catalogo));

        }

        //ServiceProvider é um agente da injeção de dependências para prover os serviços da aplicação
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            ICatalogo catalogo = serviceProvider.GetService<ICatalogo>();
            //Pelo fato do Catalogo também ser um serviço gerenciado pela injeção de dependências ele não precisa ser passado como parâmetro obrigatório
            //no construtor da classe Relatorio

            IRelatorio relatorio = serviceProvider.GetService<IRelatorio>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await relatorio.Imprimir(context);
                });
            });
        }
    }
}
