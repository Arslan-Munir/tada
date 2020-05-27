using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tada.API.Data.Interfaces;
using Tada.API.Dtos;
using Tada.API.Models;

namespace Tada.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAuthRepository _authRepository;

        public AuthController(IMapper mapper, IConfiguration config, IAuthRepository authRepository)
        {
            _mapper = mapper;
            _config = config;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDto dto)
        {
            dto.Username = dto.Username.ToLower();
            if (await _authRepository.UserExists(dto.Username))
                return BadRequest("Username already exists");

            var userToRegister = _mapper.Map<User>(dto);
            var registeredUser = await _authRepository.Register(userToRegister, dto.Password);
            var userToReturn = _mapper.Map<UserForDetailedDto>(registeredUser);
            return CreatedAtRoute("GetUser", new {controller = "Users", id = registeredUser.Id}, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDto dto)
        {
            var userFromRepo = await _authRepository.Login(dto.Username.ToLower(), dto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}