using Ecommerce.Controllers.Dto;
using Ecommerce.Controllers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Appuser> manger;
        private readonly IConfiguration conf;

        public AccountController(UserManager<Appuser> _manger, IConfiguration conf)
        {
            this.manger = _manger;
            this.conf = conf;
        }

        // to Register the users 
        [HttpPost("Register")]
        public async Task< IActionResult> Register(RegisterDto user)
        {


            if (ModelState.IsValid)
            {
                Appuser apuser = new Appuser();

                apuser.UserName = user.Username;
                apuser.PasswordHash = user.Password;
               IdentityResult created= await manger.CreateAsync(apuser,user.Password);
                if(created.Succeeded)
                {

                    // here we send ok with a message to the user 
                    return Ok("the Registeration successed");
                }
                else
                {
                    return BadRequest(created.Errors.FirstOrDefault());
                }
            }

            return BadRequest(ModelState);
        }



        // to login to the account and create token for the users 
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if(ModelState.IsValid)
            {
            Appuser userfound= await manger.FindByNameAsync(user.Username);

                if(userfound!=null)
                {
                  Boolean passok= await manger.CheckPasswordAsync(userfound,user.Password);
                    if (passok==true)
                    {
                        // here the user is found and the password is correct 
                        // ______________________________________________________________________________________________________
                        // for creating the list of the claims to be used in the jwt security token 
                        
                        List<Claim> myclaims=new List<Claim>();
                        Claim first=new Claim(ClaimTypes.Name,user.Username);

                        // here we will give it the id of the token itself and it must be unique
                        Claim second = new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString());
                        myclaims.Add(second);
                        myclaims.Add(first);

                        // ________________________________________________________________________________________________________
                        // for key
                        SecurityKey mykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["jwt:Secret"]));
                        SigningCredentials mysignincredintial=new SigningCredentials(mykey, SecurityAlgorithms.HmacSha256);
                        // 
                        //__________________________________________________________________________________________________________

                        // for creating the token   

                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: conf["jwt:issuer"],
                            audience : conf["jwt:audiance"],expires:DateTime.Now.AddDays(1),

                            claims:myclaims,
                            signingCredentials:mysignincredintial
                            );
                        // ____________________________________________________________________________________________________________________
                        // here we are finished from the creation of the token then we must use the handler of it to use it 
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo
                        }

                            ); 
                    }
                    else
                    {
                        return BadRequest("the password you enterd is not valid");
                    }
                }
                else
                {
                    return BadRequest("this user cant be found ");
                }
            }
            return BadRequest(ModelState);
        }

    }
}
