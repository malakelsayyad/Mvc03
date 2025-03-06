using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
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
    }
}
