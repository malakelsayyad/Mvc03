using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.DAL.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace Company.G02.PL.Controllers
{
    //Mvc Controller
    [Authorize]

    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //Ask Clr to create an object from DepartmentRepository
        public DepartmentController(/*IDepartmentRepository departmentRepository,*/IUnitOfWork unitOfWork,
            IMapper mapper)

        {
            _unitOfWork = unitOfWork;
            //_departmentRepository=departmentRepository;
            _mapper = mapper;
        }

        [HttpGet] // Get : /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();


            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if ( ModelState.IsValid) //Server side validation
            {
                //var department=new Department() 
                //{
                //  Code = model.Code,
                //  Name = model.Name,
                //  CreatedAt = model.CreatedAt,
                //};
                var department = _mapper.Map<Department>(model);
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    TempData["Message"] = "Department is created";
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
        public async Task<IActionResult> Details(int? id , string viewName="Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, message = $"Department with id {id} is not found" });
            var dto = _mapper.Map<CreateDepartmentDto>(department);

            return View(viewName,dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, message = $"department with id {id} is not found" });

            //var departmentDto = new CreateDepartmentDto()
            //{
            //    Code=department.Code,
            //    Name = department.Name,
            //    CreatedAt=department.CreatedAt,
            //};
            var departmentDto = _mapper.Map<CreateDepartmentDto>(department);

            return View(departmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest();
                //var department = new Department()
                //{
                //    Id = id,
                //    Name = model.Name,
                //    Code = model.Code,
                //    CreatedAt = model.CreatedAt,
                //};
                var department =_mapper.Map<Department>(model);
                department.Id = id;
                _unitOfWork.DepartmentRepository.Update(department);
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
        //        var count = _departmentRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(updateDepartment);
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(int? id) 
        {
            //if (id is null) return BadRequest("Invalid Id");
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department with id {id} is not found" });
            return await Details(id,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != department.Id) return BadRequest();
                //var department = new Department()
                //{
                //    Id = id,
                //    Name = model.Name,
                //    Code = model.Code,
                //    CreatedAt = model.CreatedAt,
                //};
                var department = _mapper.Map<Department>(model);
                department.Id = id;
                 _unitOfWork.DepartmentRepository.Delete(department);
                var count =await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

    }
}
