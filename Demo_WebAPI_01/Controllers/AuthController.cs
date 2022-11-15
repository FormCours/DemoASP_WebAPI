using Demo_WebAPI_01.BLL.Interfaces;
using Demo_WebAPI_01.BLL.Models;
using Demo_WebAPI_01.Models;
using Demo_WebAPI_01.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo_WebAPI_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private IMemberService _MemberService;
        private JwtAuthentificationService _JwtAuthentificationService;

        public AuthController(IMemberService memberService, JwtAuthentificationService jwtAuthentificationService)
        {
            _MemberService = memberService;
            _JwtAuthentificationService = jwtAuthentificationService;
        }


        [HttpPost("Register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] MemberRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                _MemberService.Register(new Member()
                {
                    Pseudo = registerDTO.Pseudo,
                    Email = registerDTO.Email,
                    HashPwd = registerDTO.Pwd,
                    Role = "User"
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }


        [HttpPost("Login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] MemberLoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Member? member = _MemberService.Login(loginDTO.Pseudo, loginDTO.Pwd);

            if (member is null)
                return BadRequest();

            AuthDTO authDTO = new AuthDTO()
            {
                Pseudo = member.Pseudo,
                Token = _JwtAuthentificationService.CreateToken(member)
            };

            return Ok(authDTO);
        }

    }
}
