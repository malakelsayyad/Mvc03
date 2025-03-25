using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Models.Dtos
{
    public class CreateDepartmentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Code is required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is required!")]
        public DateTime CreatedAt { get; set; }
    }
}
