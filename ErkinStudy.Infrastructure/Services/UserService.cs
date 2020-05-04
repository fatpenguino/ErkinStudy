using System;
using System.Collections.Generic;
using System.Linq;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ErkinStudy.Infrastructure.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public UserService(AppDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public string GetFullNameByUserName(string username)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            return $"{user?.FirstName}";
        }
        public string GetFullNameByUserId(long id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            return $"{user?.FirstName}";
        }
        public List<ApplicationUser> GetAllTeachers()
        {
            var teacherRole = _context.Roles.First(x => x.Name == "Teacher");
            var userRoles = _context.UserRoles.Where(x => x.RoleId == teacherRole.Id);
            return _context.Users.Where(x => userRoles.Any(r => r.UserId == x.Id)).ToList();
        }

        public string GetUserPhone(long userId)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
                return user == null ? _configuration.GetSection("DefaultSettings")["Phone"] : user.PhoneNumber.Replace("+","").Replace(" ","");
        }
    }
}
