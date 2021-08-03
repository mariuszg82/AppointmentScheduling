﻿using AppointmentScheduling.Models;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }
            if (_db.Roles.Any(x => x.Name == Helper.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser { 
                UserName = "email@address.com",
                Email = "email@address.com",
                EmailConfirmed = true,
                Name = "Admin Admin"
            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "email@address.com");
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();

        }
    }
}
