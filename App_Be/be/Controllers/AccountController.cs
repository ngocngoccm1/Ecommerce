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
using System.Security.Claims;
using App.Helpers;
using App.DTO.Order;

namespace App.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _sign;
        private readonly IAddressRepo _address;
        private readonly IReviewRepository _review;
        public AccountController(IAddressRepo address, UserManager<User> userManager,
         ITokenService tokenService, SignInManager<User> sign, IReviewRepository review)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _sign = sign;
            _address = address;
            _review = review;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Tên đăng nhập không hợp lệ");

            var result = await _sign.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,

                Token = _tokenService.CreateToken(user, user.NormalizedUserName)
            }
            );

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
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
                    var role = "User";
                    var roleResult = await _userManager.AddToRoleAsync(user, role);
                    if (roleResult.Succeeded)
                    {
                        await _address.CreateAsync(user.UserName);

                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user, role)
                            }
                            );
                    }
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
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userManager.Users.Include(us => us.CartItems).Include(us => us.Orders).ToListAsync();

            var userwithCart = new List<User_CartDto>();
            foreach (var user in users)
            {
                userwithCart.Add(new User_CartDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Orders = user.Orders,
                    CartItems = user.CartItems
                });
            }


            return Ok(userwithCart);
        }

        [HttpGet]
        [Route("/IsAdmin")]
        //[Authorize(Roles = "User")]
        [Authorize(Roles = "ADMIN")]

        public IActionResult CheckAdmin()
        {
            return Ok("I'm admin");
        }
        [HttpPost]
        [Route("/Review")]
        [Authorize]
        public async Task<IActionResult> Review(int id, string content)
        {
            var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rep = await _review.CreateAsync(id, id_user, content);
            return Ok(rep);
        }
        
        [HttpPost]
        [Route("/like")]
        [Authorize]
        public async Task<IActionResult> like(string id)
        {
            string id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _review.like(id, id_user);
            return Ok("ok");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users.Include(u => u.Address).Include(u => u.Reivews)
            .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(user); // Hoặc xử lý theo cách bạn muốn
        }


        [HttpPut("Edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromForm] UserUpdate u)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.Users.Include(us => us.Address)
                            .FirstOrDefaultAsync(x => x.Id == id);

            user.Address.Street = u.Street;
            user.Address.City = u.City;
            user.Address.State = u.State;
            user.Address.ZipCode = u.ZipCode;
            user.Gender = u.Gender;
            user.Email = u.Email;
            user.FullName = u.FullName;
            user.PhoneNumber = u.PhoneNumber;
            user.ProfilePictureUrl = ImageHelper.EncodeImageToBase64(u.ProfilePictureUrl);

            await _userManager.UpdateAsync(user);
            return Ok(user); // Hoặc xử lý theo cách bạn muốn
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            // Check if the user exists
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Attempt to delete the user
            var result = await _userManager.DeleteAsync(user);

            // Check if the deletion was successful
            if (!result.Succeeded)
            {
                return BadRequest("Failed to delete user.");
            }

            return Ok("User deleted successfully.");
        }

    }

}