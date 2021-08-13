using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LimFx.Business.Exceptions;
using LimFx.Business.Extensions;
using LimFx.Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using PornJudger.Dto;
using PornJudger.Models;

namespace PornJudger
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


            services.AddPredictionEnginePool<ImageInput, ModelOutput>()
                .FromFile("MLModel.zip", true);


            services.AddControllers();


            services.AddAuthentication(op =>
            {
                op.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(op =>
                {
                    op.Events.OnRedirectToLogin += (o) => throw new _403Exception("无法通过身份验证");
                    op.Events.OnRedirectToAccessDenied += (o) => throw new _403Exception("无法通过身份验证");
                });
            services.AddAuthorization(op =>
            {
                op.InvokeHandlersAfterFailure = false;
            });

            // 添加路径ratelimit
            services.AddEnhancedRateLimiter(ClaimTypes.UserData, 1000, 1000);

            // cookie持久化
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), ".secret")))
                .SetDefaultKeyLifetime(new TimeSpan(50, 0, 0, 0));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // 添加limfx错误处理中间件 https://chronos.limfx.pro/ReadArticle/457/limfxerrorhandler-shi-yong-wen-dang
            app.UseLimFxExceptionHandler();

            app.UseStaticFiles(new StaticFileOptions 
            {
                ServeUnknownFileTypes = true
            });
            // 添加全局ratelimiter
            app.UseRateLimiter(maxRequest: 20, blockTime: 10000, maxReq: 200);


            app.UseRouting();

            app.UseAuthentication();

            app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
