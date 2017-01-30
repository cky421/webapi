using System; 
using System.Collections.Generic; 
using System.Linq;
using Microsoft.AspNetCore.Mvc; 
using TheOne.Common.Auth; 
using System.IdentityModel.Tokens.Jwt; 
using Newtonsoft.Json; 
using System.Security.Claims; 
using System.Security.Principal; 
using Microsoft.IdentityModel.Tokens; 
using TheOne.Models; 
using Microsoft.AspNetCore.Authorization; 
 
namespace OneThing.Controllers 
{ 
    [Route("api/[controller]")] 
    public class TokenAuthController : Controller 
    { 
        [HttpPost] 
        public string POST([FromBody]User user) 
        { 
            var existUser = UserStorage.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password); 
 
            if (existUser != null) 
            { 
                var requestAt = DateTime.Now; 
                var expiresIn = requestAt + TokenAuthOption.ExpiresSpan; 
                var token = GenerateToken(existUser, expiresIn); 
 
                return JsonConvert.SerializeObject(new Response 
                { 
                    State = RequestState.Success, 
                    Data = new 
                    { 
                        requertAt = requestAt, 
                        expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds, 
                        tokeyType = TokenAuthOption.TokenType, 
                        accessToken = token 
                    } 
                }); 
            } 
            else 
            { 
                return JsonConvert.SerializeObject(new Response 
                { 
                    State = RequestState.Failed, 
                    Msg = "Username or password is invalid" 
                }); 
            } 
        } 
 
        private string GenerateToken(User user, DateTime expires) 
        { 
            var handler = new JwtSecurityTokenHandler(); 
 
            ClaimsIdentity identity = new ClaimsIdentity( 
                new GenericIdentity(user.Username, "TokenAuth"), 
                new[] { 
                    new Claim("ID", user.ID.ToString()) 
                } 
            ); 
 
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor 
            { 
                Issuer = TokenAuthOption.Issuer, 
                Audience = TokenAuthOption.Audience, 
                SigningCredentials = TokenAuthOption.SigningCredentials, 
                Subject = identity, 
                Expires = expires 
            }); 
            return handler.WriteToken(securityToken); 
        } 
 
        [HttpGet] 
        [Authorize("Bearer")] 
        public string GET() 
        { 
            var claimsIdentity = User.Identity as ClaimsIdentity; 
 
            return JsonConvert.SerializeObject(new Response 
            { 
                State = RequestState.Success, 
                Data = new 
                { 
                    UserName = claimsIdentity.Name 
                } 
            }); 
        } 
    } 
 
    public static class UserStorage 
    { 
        public static List<User> Users { get; set; } = new List<User> { 
            new User {ID=Guid.NewGuid(),Username="user1",Password = "user1psd" }, 
            new User {ID=Guid.NewGuid(),Username="user2",Password = "user2psd" }, 
            new User {ID=Guid.NewGuid(),Username="user3",Password = "user3psd" } 
        }; 
    } 
} 