using Brive.Bootcamp.login.Helpers;
using LoginProveedor.Model;
using LoginProveedor.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginProveedor.Controllers
{
    [EnableCors("Login")]
    [Route("login/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IServices _services;
        private readonly IConfiguration _conf;
        public AdminController(IServices services, IConfiguration config)
        {
            _conf = config;
            _services = services;
        }

        public ActionResult<LoginAdmin[]> Get()
        {
            return Ok(_services.getAdmins());
        }

        [HttpPost]
        public IActionResult Post([FromBody] AdminAccount userAccount)
        {
            if (userAccount.Email == null || userAccount.Password == null
                    || userAccount.Email == "" || userAccount.Password == "" || !_services.VerifyEmail(userAccount.Email))
            {
                return BadRequest(_services.messageResponse(400, "Missing something"));
            }

            if (!_services.verifyAccount(userAccount.Email, userAccount.Password))
            {
                return NotFound(_services.messageResponse(404, "Incorrect email or password"));
            }
            string secret = this._conf.GetValue<string>("Secret");
            var jwtHelper = new JWTHelper(secret);
            var token = jwtHelper.createToken(userAccount.Email);

            return Accepted(_services.messageResponse(202, token));
        }

        [HttpPost("register")]
        public ActionResult Post([FromBody] LoginAdmin user)
        {
            if (user.Email == "" || user.Email == null || user.ApellidoM == "" || user.ApellidoM == null || user.ApellidoP == ""
                || user.ApellidoP == null || user.Password == "" || user.Password == null)
            {

                return BadRequest(_services.messageResponse(400, "Some is missing"));
            }

            if (_services.VerifyEmail(user.Email))
            {
                if (_services.VerifyPassword(user.Password))
                {
                    return
                        _services.SaveUser(user) ? // hasheo en SaveUser
                        Created("/login/Users/register", new { status = 201, information = "Done" })
                        :
                        BadRequest(_services.messageResponse(400, "Email ya registrado"));
                }

                return BadRequest(_services.messageResponse(400, "Password invalido"));
            }

            return BadRequest(_services.messageResponse(400, "Email invalido"));
        }

        [HttpPost("password")]
        public ActionResult Post(string password, string confirmPassword)
        {
            return Ok(new { status = 200, hash = _services.hashPassword(password) });
        }

        [HttpPut("update")]
        public IActionResult Put([FromBody] AdminUpdatePassword user)
        {
            if (_services.EmailExist(user.Email))
            {
                if (user.Password == user.ConfirmPassword)
                {
                    if (_services.PasswordNotTheSame(user.Email, _services.hashPassword(user.Password)) && _services.VerifyPassword(user.Password))
                    {

                        _services.UpdatePassword(user.Email, user.Password);
                        return Ok(_services.messageResponse(200, "Updated password"));
                    }

                    return BadRequest(_services.messageResponse(400, "Wrong password")); // el password nuevo es el mismo al viejo
                }

                return BadRequest(_services.messageResponse(400, "Not match password")); // las contraseñas no son iguales
            }

            return NotFound(_services.messageResponse(404, "Email not found"));
        }
    }
}
