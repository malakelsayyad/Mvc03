﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string  Email { get; set; }
        public string  Address { get; set; }
        public string  Phone { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime CreatedAt { get; set; }
        [DisplayName("Department")]
        public int? DepartmentId  { get; set; }
        public Department? Department { get; set; }
        public string? ImageName { get; set; }
    }
}
