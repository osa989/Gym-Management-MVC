 using GymManagementDAL.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage = "Photo Is Required")]
        [Display(Name = "Profile Photo")]
        public IFormFile PhotoFile { get; set; } = null!;



        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must be Between 2 And 50 Characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can contain only letters and spaces.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email Is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be Between 5 And 100 Characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must be valid eyption format phone number")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Date Of Birth Is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, 1000, ErrorMessage = "Building Number Must be Between 1 And 1000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name Must be Between 2 And 30 Char")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 And 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can Contain only Letters and Spaces")]
        public string City { get; set; } = null!;
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
    }
}
