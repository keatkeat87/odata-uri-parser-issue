using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.ModelBuilder;
using test_odata.Pages;

namespace test_odata
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<UserSettingGroup>("UserSettingGroups");

            services.AddRazorPages();
            // for test preview 3
            //services.AddRouting();
            //services.AddOData(opt =>
            //{
            //    opt.SetTimeZoneInfo(TimeZoneInfo.Utc);
            //    opt.Select().Expand().Filter().OrderBy().SetMaxTop(null).Count();
            //    opt.AddModel("api", builder.GetEdmModel());
            //});

            services.AddControllers().AddOData(opt =>
            {
                opt.SetTimeZoneInfo(TimeZoneInfo.Utc);
                opt.Select().Expand().Filter().OrderBy().SetMaxTop(null).Count();
                opt.AddModel("api", builder.GetEdmModel());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
