using Library.Library.Data;
using Library.Library.Entities;
using LibraryManagement.UI.Models.Storage;
using LibraryManagement.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;

namespace LibraryManagement.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine(">>> Đang kiểm tra xem đang chạy trên máy tính của ai...");
            var whatPC = Path.Combine($"{WebHostEnvironment.ContentRootPath}\\Properties\\WhatPC");
            if (File.Exists($"{whatPC}\\thuan.dh19pm"))
            {
                services.AddDbContext<LibraryDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("ConnectLibrary"));
                });
                Console.WriteLine(">>> Đang chạy trên máy tính của [Võ Thành Thuận]");
            }
            else
            {
                services.AddDbContext<LibraryDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("ConnectLibraryOfSon"));
                });
                Console.WriteLine(">>> Đang chạy trên máy tính của [Nguyễn Ngọc Sơn]");

            }

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<LibraryDbContext>()
                    .AddDefaultTokenProviders();
            services.AddTransient<IStorageService, FileService>();
            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddTransient<RoleManager<Role>, RoleManager<Role>>();

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
