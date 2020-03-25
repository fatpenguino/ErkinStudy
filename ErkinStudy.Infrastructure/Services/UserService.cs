using System.Collections.Generic;
using System.Linq;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Infrastructure.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
    }
}
