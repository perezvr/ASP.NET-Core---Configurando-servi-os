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
            //Configurando a aplica��o para forncecer os servi�os

            //Cria um servi�o transit�rio (ser� gerada uma nova inst�ncia sempre que esse servi�o for chamado, em cada m�todo)
            //services.AddTransient<ICatalogo, Catalogo>();
            //services.AddTransient<IRelatorio, Relatorio>();

            //Desta forma, s� ser� gerada uma inst�ncia dos servi�os para cada requisi��o
            services.AddScoped<ICatalogo, Catalogo>();
            services.AddScoped<IRelatorio, Relatorio>();

            //Com o singleton teremos uma inst�ncia �nica a cada execu��o da aplica��o
            //var catalogo = new Catalogo();
            //services.AddSingleton<ICatalogo>(catalogo);
            //services.AddSingleton<IRelatorio>(new Relatorio(catalogo));

        }

        //ServiceProvider � um agente da inje��o de depend�ncias para prover os servi�os da aplica��o
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            ICatalogo catalogo = serviceProvider.GetService<ICatalogo>();
            //Pelo fato do Catalogo tamb�m ser um servi�o gerenciado pela inje��o de depend�ncias ele n�o precisa ser passado como par�metro obrigat�rio
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
