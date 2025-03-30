using System.ComponentModel.DataAnnotations;

namespace Company.G02.DAL.Models.Dtos
{
    public class ResetPasswordDto
    {

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")] 

        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
