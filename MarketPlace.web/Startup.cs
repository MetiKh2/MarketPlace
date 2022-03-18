using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using MarketPlace.application.Services.Impelimentions;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.Context;
using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Repository;
using MarketPlace.web.Areas.Store.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MarketPlace.web
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
            services.AddControllersWithViews();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<IContactService,ContactService>();
            services.AddScoped<IStoreService,StoreService>();
            services.AddScoped<IPaymentService,PaymentService>();
            services.AddScoped<IStoreWalletService, StoreWalletService>();
            services.AddScoped<IDiscountService,DiscountService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
            services.AddScoped<PanelFilter>();

            #region ConfigDataBase
            services.AddDbContext<MarketPlaceDbContext>(op =>
            {
                op.UseSqlServer(Configuration.GetConnectionString("MarketPlaceConnection"));
            });
            #endregion

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                op.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(op=> {
                op.LoginPath = "/login";
                op.LogoutPath = "/Logout";
                op.ExpireTimeSpan = TimeSpan.FromDays(30);
                 
            });

            #region Html Encoder
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(new[] { UnicodeRanges.BasicLatin,UnicodeRanges.Arabic}));
            #endregion
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
         );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
