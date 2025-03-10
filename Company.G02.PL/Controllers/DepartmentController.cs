using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.DAL.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace Company.G02.PL.Controllers
{
    //Mvc Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        //Ask Clr to create an object from DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)

        {
            _departmentRepository=departmentRepository;
        }

        [HttpGet] // Get : /Department/Index
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();


            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if ( ModelState.IsValid) //Server side validation
            {   
                var department=new Department() 
                {
                  Code = model.Code,
                  Name = model.Name,
                  CreatedAt = model.CreatedAt,
                };
              var count= _departmentRepository.Add(department);
                if (count > 0) 
                {
                  return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        #region Task 03 (Details action and view)
        //[HttpGet]
        //public IActionResult Details(DetailsDepartmentDto departmentDto)
        //{
        //    var department = _departmentRepository.GetById(departmentDto.Id);
        //    var departmentDetailsDto = new DetailsDepartmentDto
        //    {
        //        Code = department.Code,
        //        Name = department.Name,
        //        CreatedAt = department.CreatedAt,
        //    };

        //    return View(departmentDetailsDto);
        //}
        #endregion

        [HttpGet]
        public IActionResult Details(int? id , string viewName="Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _departmentRepository.GetById(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, message = $"Department with id {id} is not found" });
            return View(viewName,department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = _departmentRepository.GetById(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, message = $"department with id {id} is not found" });

            var departmentDto = new CreateDepartmentDto()
            {
                Code=department.Code,
                Name = department.Name,
                CreatedAt=department.CreatedAt,
            };
            return View(departmentDto);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest();
                var department = new Department()
                {
                    Id = id,
                    Name = model.Name,
                    Code = model.Code,
                    CreatedAt = model.CreatedAt,
                };
                var count = _departmentRepository.Update(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
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
        //        var count = _departmentRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(updateDepartment);
        //}

        [HttpGet]
        public IActionResult Delete(int? id) 
        {
            //if (id is null) return BadRequest("Invalid Id");
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with id {id} is not found" });
            return Details(id,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest();
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

    }
}
