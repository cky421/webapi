﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Common;
using WebApi.Models;
using Xunit;

namespace WebApi.Tests.IntegrationTests
{
    public class AuthControllerTests :IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient client;

        public AuthControllerTests(TestFixture<Startup> fixture)
        {
            client = fixture.Client;
        }

        [Fact]
        public async Task TestGetAuthToken()
        {
            User user = new User()
            {
                Username = Config.AdminName,
                Password = Config.AdminPwd
            };
            var response = await client.PostAsJsonAsync("/api/v1/auth", user);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsJsonAsync<Response>();
            Assert.Equal(ResponseState.Success, responseContent.state);
        }
    }
}