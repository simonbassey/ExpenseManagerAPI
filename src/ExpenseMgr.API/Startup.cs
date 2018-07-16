using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseMgr.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExpenseMgr.Services.Abstractions;
using ExpenseMgr.Services;
using ExpenseMgr.Data.Repositories;
using ExpenseMgr.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ExpenseMgr.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ExpenseMgrContext>();
            services.AddEntityFrameworkSqlite();

            services.AddCors(options => options.AddPolicy("*", corsPolicyBuilder => corsPolicyBuilder
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin()));
            services.Configure<IISOptions>(config =>
                                           config.ForwardClientCertificate = false);

            //Configuring dependency injection
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICurrencyConverter, CurrencyConverter>();
            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("*");
            app.UseMvc();
            var context = ((ExpenseMgrContext)app.ApplicationServices.GetService(typeof(ExpenseMgrContext)));
            context.Database.Migrate();
        }
    }
}
