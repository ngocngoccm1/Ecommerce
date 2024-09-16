using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Identity;
using App.DTO.Account;
using App.Interface;
using App.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _sign;
        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> sign)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _sign = sign;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Tên đăng nhập không hợp lệ");

            var result = await _sign.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user, _userManager.GetRolesAsync(user).ToString())
                }
            );

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var user = new User
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };
                var createUser = await _userManager.CreateAsync(user, registerDto.Password);
                if (createUser.Succeeded)
                {
                    var role = "Admin";
                    var roleResult = await _userManager.AddToRoleAsync(user, role);
                    if (roleResult.Succeeded)
                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user, role)
                            }
                            );
                    else return StatusCode(500, roleResult.Errors);
                }
                else return StatusCode(500, createUser.Errors);

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        [Route("/manager")]
        //[Authorize(Roles = "User")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();
            var userwithRoles = new List<UserDto>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                userwithRoles.Add(new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = (List<string>)userRoles
                });
            }


            return Ok(userwithRoles);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserRoles()
        {
            if (User.IsInRole("User"))
            {
                return Ok("roles"); // Hoặc xử lý theo cách bạn muốn

            }


            return Ok("no"); // Hoặc xử lý theo cách bạn muốn
        }

    }

}