using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Models.Dtos;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        //Ask Clr to create an object from IEmployeeRepository
        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )

        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet] // Get : /Department/Index
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }
            // Dictionary : 3 Properties
            // 1.ViewData : Transfer Extra Info from controller to view
            //ViewData["Message"] = "Hello from ViewData";

            // 2.ViewBag : Transfer Extra Info from controller to view
            //ViewBag.Message = new { Message = "Hello from ViewBag" };



            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
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
                if (model.Image is not null)
                {
                 model.ImageName= DocumentSettings.UploadFile(model.Image, "Images");

                }

                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                 
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is created";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee with id {id} is not found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id");
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
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest();
                //var employee = new Employee()
                //{   Id=id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreatedAt = model.CreatedAt,
                //    HiringDate = model.HiringDate,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    DepartmentId = model.DepartmentId,
                //};
               
                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                }

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");

                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                 _unitOfWork.EmployeeRepository.Update(employee);
                 var count = await _unitOfWork.CompleteAsync();

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
        public Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id");
            //var department = _employeeRepository.GetById(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with id {id} is not found" });
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest();
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                 _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    if (model.ImageName is not null)
                    { 
                       DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

    }
}