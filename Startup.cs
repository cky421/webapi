using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TheOne.Common.Auth;
using TheOne.Models;
using TheOne.Repositories;

namespace TheOne
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
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder() 
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build()); 
            }); 
            services.AddMvc();
            services.AddSwaggerGen();
            services.AddSingleton<IUserRepository, UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseExceptionHandler(appBuilder => 
            { 
                appBuilder.Use(async (context, next) => 
                { 
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature; 
                    // 认证失败 
                    if (error != null && error.Error is SecurityTokenExpiredException) 
                    { 
                        context.Response.StatusCode = 401; 
                        context.Response.ContentType = "application/json"; 

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response 
                        { 
                            State = RequestState.NotAuth, 
                            Msg = "token expired" 
                        })); 
                    } 
                    // 服务器内部错误
                    else if (error != null && error.Error != null) 
                    { 
                        context.Response.StatusCode = 500; 
                        context.Response.ContentType = "application/json"; 
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response 
                        { 
                            State = RequestState.Failed, 
                            Msg = error.Error.Message 
                        })); 
                    } 
                    // 没有错误 
                    else await next(); 
                }); 
            }); 


            app.UseJwtBearerAuthentication(new JwtBearerOptions() 
            { 
                TokenValidationParameters = new TokenValidationParameters() 
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

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
