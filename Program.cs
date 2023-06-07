using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SnackisProject.Areas.Identity.Data;
using SnackisProject.Gateways;

namespace SnackisProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("SnackisUserContextConnection") ?? throw new InvalidOperationException("Connection string 'SnackisUserContextConnection' not found.");

            builder.Services.AddDbContext<SnackisUserContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<SnackisUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<SnackisUserContext>();


            builder.Services.AddScoped<CategoryGateway>();
            builder.Services.AddScoped<SubCategoryGateway>();
            builder.Services.AddScoped<PostGateway>();
            builder.Services.AddScoped<CommentGateway>();
            builder.Services.AddScoped<ReportGateway>();
            builder.Services.AddScoped<MessageGateway>();

            //builder.Services.AddTransient<CategoryGateway>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("JustAdmin", policy => policy.RequireRole("Admin")); // För admin
            });



            builder.Services.AddRazorPages(options =>
            {
                //options.Conventions.AuthorizeFolder("/Secret");
                options.Conventions.AuthorizeFolder("/Admin", "JustAdmin");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}