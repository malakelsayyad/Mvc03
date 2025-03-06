using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.DAL.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

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
            if (ModelState.IsValid) //Server side validation
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
    }
}
