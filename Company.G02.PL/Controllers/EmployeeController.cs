using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models.Dtos;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        //Ask Clr to create an object from IEmployeeRepository
        public EmployeeController(IEmployeeRepository employeeRepository)

        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet] // Get : /Department/Index
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();


            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) //Server side validation
            {
               var employee = new Employee() 
               {
                 Name = model.Name,
                 Address = model.Address,
                 Age = model.Age,
                 CreatedAt = model.CreatedAt,
                 HiringDate = model.HiringDate,
                 Email = model.Email,
                 IsActive = model.IsActive,
                 IsDeleted = model.IsDeleted,
                 Phone = model.Phone,
                 Salary = model.Salary,
               };

                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee with id {id} is not found" });
            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"employee with id {id} is not found" });

            var employeeDto = new CreateEmployeeDto()
            {
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                CreatedAt = employee.CreatedAt,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                Phone = employee.Phone,
                Salary = employee.Salary,
            };
            return View(employeeDto);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest();
                var employee = new Employee()
                {   Id=id,
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreatedAt = model.CreatedAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary,
                };
                var count = _employeeRepository.Update(employee);
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
        //        var count = _employeeRepository.Update(department);
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
            //var department = _employeeRepository.GetById(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with id {id} is not found" });
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                var count = _employeeRepository.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }

    }
}
