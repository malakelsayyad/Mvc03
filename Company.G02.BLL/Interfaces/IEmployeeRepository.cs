using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        Task<List<Employee>> GetByNameAsync(string name);

        //IEnumerable<Employee> GetAll();
        //Employee? GetById(int id);
        //int Add(Employee employee);
        //int Update(Employee employee);
        //int Delete(Employee employee);
    }
}
