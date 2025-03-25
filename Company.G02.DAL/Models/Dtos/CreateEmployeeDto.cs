﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Models.Dtos
{
    public class CreateEmployeeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required !")]
        public string Name { get; set; }
        
        [Range(22,60, ErrorMessage ="Age must be between 22 and 60")]
        public int? Age { get; set; }

        [DataType(DataType.EmailAddress,ErrorMessage ="Email is not valid")]
        public string Email { get; set; }

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
                          , ErrorMessage = "Address must be like 123-street-city-country")]
        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }

        [DisplayName("Date of creation")]
        public DateTime CreatedAt { get; set; }
        
        [DisplayName("Department")]
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
