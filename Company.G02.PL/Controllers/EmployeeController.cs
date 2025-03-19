using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models.Dtos;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        //Ask Clr to create an object from IEmployeeRepository
        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper
            )

        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet] // Get : /Department/Index
        public IActionResult Index(string? SearchInput)
        {
             IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = _employeeRepository.GetAll();

            }
            else
            { 
              employees = _employeeRepository.GetByName(SearchInput);
            }
            // Dictionary : 3 Properties
            // 1.ViewData : Transfer Extra Info from controller to view
            //ViewData["Message"] = "Hello from ViewData";

            // 2.ViewBag : Transfer Extra Info from controller to view
            //ViewBag.Message = new { Message = "Hello from ViewBag" };



            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"]= departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) //Server side validation
            {
                //Manual mapping
                //var employee = new Employee() 
                //{
                //  Name = model.Name,
                //  Address = model.Address,
                //  Age = model.Age,
                //  CreatedAt = model.CreatedAt,
                //  HiringDate = model.HiringDate,
                //  Email = model.Email,
                //  IsActive = model.IsActive,
                //  IsDeleted = model.IsDeleted,
                //  Phone = model.Phone,
                //  Salary = model.Salary,
                //  DepartmentId = model.DepartmentId,
                //};

                var employee = _mapper.Map<Employee>(model);
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is created";
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
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"employee with id {id} is not found" });

            //var employeeDto = new CreateEmployeeDto()
            //{
            //    Name = employee.Name,
            //    Address = employee.Address,
            //    Age = employee.Age,
            //    CreatedAt = employee.CreatedAt,
            //    HiringDate = employee.HiringDate,
            //    Email = employee.Email,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    DepartmentId = employee.DepartmentId,
            //};
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest();
                var employee = new Employee()
                {   Id=id,
                    Name = model.EmpName,
                    Address = model.Address,
                    Age = model.Age,
                    CreatedAt = model.CreatedAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    DepartmentId = model.DepartmentId,
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
