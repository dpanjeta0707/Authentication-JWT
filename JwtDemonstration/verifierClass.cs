using JwtDemonstration.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtDemonstration
{
    public class verifierClass
    {
    
            public string LoginUser(string UserID, string Password)
            {
                
                var user = UserList.SingleOrDefault(x => x.USERID == UserID);

                if (user == null)
                    return null;

                if (Password == user.PASSWORD)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                        new Claim("Student", "Cookie")
                    };


                    var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
                    var key = new SymmetricSecurityKey(secretBytes);
                    var algorithm = SecurityAlgorithms.HmacSha256;

                    var signingCredentials = new SigningCredentials(key, algorithm);

                    var token = new JwtSecurityToken(
                        Constants.Issuer,
                        Constants.Audience,
                        claims,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials);

                    var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
                    return tokenJson;
                }
                else
                {
                    return null;
                }
            }

            private List<Users> UserList = new List<Users>
            {
                new Users { USERID = "bdat_student",
                PASSWORD = "test", EMAILID = "student@student.com",
                FIRST_NAME = "John", LAST_NAME = "Smith",
                PHONE = "356-735-2748", ACCESS_LEVEL = "Director",
                READ_ONLY = "true" },
                new Users { USERID = "other_bdat_student", PASSWORD = "test",
                FIRST_NAME = "Steve", LAST_NAME = "Rob",
                EMAILID = "other@student.com", PHONE = "567-479-8537",
                ACCESS_LEVEL = "Supervisor", READ_ONLY = "false" }
            };

          
          
    }
}

