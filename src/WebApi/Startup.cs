using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Common.Auth;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(Config.IdentityType, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
            services.AddCors();
            services.AddMvc();
            services.AddSwaggerGen();
            services.AddSingleton<IUserRepository, UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("Logs/webapi-{Date}.log");
            loggerFactory.AddDebug();

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    // 认证失败 
                    if (error?.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response
                        {
                            state = ResponseState.NotAuth,
                            msg = "token expired"
                        }));
                    }
                    // 服务器内部错误
                    else if (error?.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response
                        {
                            state = ResponseState.Failed,
                            msg = error.Error.Message
                        }));
                    }
                    // 没有错误 
                    else await next();
                });
            });


            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    // 验证签名密钥是否正确
                    ValidateIssuerSigningKey = true,
                    // 验证令牌是否超出有效期
                    ValidateLifetime = true,
                    // 令牌的时间偏差。增加令牌的有效期以方便认证。此处设为0。
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });

            app.UseCors(builder =>
                builder.WithOrigins(Config.Origins).AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
            );

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
