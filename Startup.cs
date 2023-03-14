using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkinsSite.Context;
using SkinsSite.Models;
using SkinsSite.Repositories;
using SkinsSite.Repositories.Interfaces;
using SkinsSite.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ReflectionIT.Mvc.Paging;
using SkinsSite.Areas.Admin.Services;

namespace SkinsSite;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<ConfigurationImagens>(Configuration.GetSection("ConfigurationPastaImagens"));

        //services.Configure<IdentityOptions>(options =>
        //{
        //    //Default Password settings.
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireLowercase = false;
        //    options.Password.RequireNonAlphanumeric = false;
        //    options.Password.RequireUppercase = false;
        //    options.Password.RequiredLength = 8;
        //    options.Password.RequiredUniqueChars = 0;
        //});

        services.AddTransient<ISkinRepository, SkinRepository>();
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddTransient<IPedidoRepository, PedidoRepository>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
        services.AddScoped<RelatorioVendasService>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                politica =>
                {
                    politica.RequireRole("Admin");
                });
        });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

        services.AddControllersWithViews();

        services.AddPaging(options =>
        {
            options.ViewName = "Bootstrap5";
            options.PageParameterName = "pageindex";
        });

        services.AddMemoryCache();
        //services.AddDistributionMemoryCache();

        services.AddSession();
        //{
        //    options.IdleTimeout = TimeSpan.FromSeconds(10);
        //    options.Cookie.HttpOnly = true;
        //    options.Cookie.IsEssential = true;
        //});
    }

    public void Configure(IApplicationBuilder app, 
        IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseRouting();

        //cria os perfis
        seedUserRoleInitial.SeedRoles();
        //cria os usuarios e atribui ao perfil
        seedUserRoleInitial.SeedUsers();

        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
             name: "areas",
             pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
               name: "categoriaFiltro",
               pattern: "Skin/{action}/{categoria?}",
               defaults: new { Controller = "Skin", action = "List" });

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}