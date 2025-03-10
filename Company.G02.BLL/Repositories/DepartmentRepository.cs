using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Data.Context;
using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        public DepartmentRepository(CompanyDbContext context) : base(context) //Ask Clr To make an object from CompanyDbContext
        {

        }
        //    private readonly CompanyDbContext _context; //Null
        //    //Asdk Clr to create an object from CompanyDbContext
        //    public DepartmentRepository(CompanyDbContext context)
        //    {
        //        _context = context;
        //    }
        //    public IEnumerable<Department> GetAll()
        //    {
        //        return _context.Departments.ToList();
        //    }
        //    public Department? GetById(int id)
        //    {
        //        return _context.Departments.Find(id);
        //    }
        //    public int Add(Department department)
        //    {
        //        _context.Add(department);
        //        return _context.SaveChanges();
        //    }
        //    public int Update(Department department)
        //    {
        //        _context.Update(department);
        //        return _context.SaveChanges();
        //    }
        //    public int Delete(Department department)
        //    {
        //        _context.Remove(department);
        //        return _context.SaveChanges();
        //    }

    }
}
