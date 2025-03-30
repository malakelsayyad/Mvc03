using Company.G02.DAL.Models.Dtos;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Company.G02.PL.Helpers;
using Company.G02.PL.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Company.G02.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> users;

            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                });

            }
            else
            {
                users = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                }).Where(U => U.Name.ToLower().Contains(SearchInput));
            }

            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid) //Server side validation
            {
                var role = await _roleManager.FindByNameAsync(model.Name);

                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name,
                    };
                }
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { statusCode = 404, message = $"User with id {id} is not found" });

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name,

            };
            return View(viewName, dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid operation");

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid operation");
                var roleResult = await _roleManager.FindByNameAsync(role.Name);

                if (roleResult is null)
                {
                    role.Name = model.Name;

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Invalid operation");

            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto updateDepartment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department() 
        //        {
        //           Id=id,
        //           Name=updateDepartment.Name,
        //           Code=updateDepartment.Code,
        //           CreatedAt=updateDepartment.CreatedAt,
        //        };
        //        var count = _employeeRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(updateDepartment);
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Task<IActionResult> Delete(string? id)
        {
            //if (id is null) return BadRequest("Invalid Id");
            //var department = _employeeRepository.GetById(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with id {id} is not found" });
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid operation");

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid operation");

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Invalid operation");

            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            ViewData["RoleId"] = roleId;

            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();


            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = roleId });
            }

            return View(users);
        }
    }

}