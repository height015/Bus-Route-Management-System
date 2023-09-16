using System.ComponentModel.DataAnnotations;
using Creative.Core.Extention;
using Swashbuckle.AspNetCore.Annotations;

namespace Models;

public class UserResponseVM {

    public string EmailAddress { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }

}

public class UserLoginVM
{
    [Required(ErrorMessage = "Email Address or User name is Required", AllowEmptyStrings =false)]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Password is Required", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
   
}

public class UserRegistrationVM {

    [StringLength(50, MinimumLength = 4, ErrorMessage = "First Name contains fewer or more characters than expected. (4 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; }

    [StringLength(50, MinimumLength = 4, ErrorMessage = "Surname contains fewer or more characters than expected. (4 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string Surname { get; set; }


    [StringLength(50, MinimumLength = 4, ErrorMessage = "Other Name contains fewer or more characters than expected. (3 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string OtherName { get; set; }

    [StringLength(50, MinimumLength = 4)]
    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    public string EmailAddress { get; set; }


    [StringLength(15, MinimumLength = 3, ErrorMessage = "User Name contains fewer or more characters than expected. (4 to 15 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; }


    [Required(AllowEmptyStrings = false)]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Phone contains fewer or more characters than expected. (11 to 15 characters expected)")]
    
    public string Phone { get; set; }


    [StringLength(50, MinimumLength = 3, ErrorMessage = "Home Address contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string HomeAddress { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(400, MinimumLength = 6, ErrorMessage = "Password contains fewer or more characters than expected. (min of 6 characters expected)")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [CheckNumber(0, ErrorMessage = "Service Type is required")]
    public int ServiceType { get; set; }

    [CheckNumber(0, ErrorMessage = "Passenger Type is required")]
    public int PassengerType { get; set; }

    public int AdultCount { get; set; }

    public int ChildrenCount { get; set; }

    [CheckNumber(0, ErrorMessage = "PickUp Point is required")]
    public int PickUpPointId { get; set; }

    [CheckNumber(0, ErrorMessage = "Route is required")]
    public int RouteId { get; set; }

    [CheckNumber(0, ErrorMessage = "LCDA is required")]
    public int LCDA { get; set; }

    [CheckNumber(0, ErrorMessage = "Gender is required")]
    public int Gender { get; set; }


    [StringLength(15, MinimumLength = 3, ErrorMessage = "Birth Date contains fewer or more characters than expected. (10 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string BirthDate { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Address Contains fewer or more characters than expected. (3 to 50 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string Address { get; set; }
}

public class UserVM
{
    public string UserId { get; set; }

    public string FirstName { get; set; }

    public string Surname { get; set; }

    public string OtherName { get; set; }

    public string EmailAddress { get; set; }

    public string UserName { get; set; }

    public string Phone { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string HomeAddress { get; set; }

    public string DateOfBirth { get; set; }

    public string LastActivityDate { get; set; }

    public string LastLoginIpAddress { get; set; }

    public string DatedJoined { get; set; }

    public string UpdatedDated { get; set; }

    public string LCDALabel { get; set; }

    public string GenderLabel { get; set; }


}


public class ChangePasswordVM
{

    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$",
  ErrorMessage = "Old Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }


    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$",
    ErrorMessage = "New Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$",
  ErrorMessage = "Confirm New Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; }

  
}


public class EditUserVM
{
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Given Name contains fewer or more characters than expected. (3 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string GivenName { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname contains fewer or more characters than expected. (3 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string Surname { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "OtherName contains fewer or more characters than expected. (3 to 50 characters expected)")]
    public string OtherName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "City contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string City { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "State contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string State { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Home Address contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(AllowEmptyStrings = false)]
    public string HomeAddress { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

}


public class UserConfirmEmail
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string Email { get; set; }
}


public class UserChangeEmail
{
    [Required]
    public string Email { get; set; }

    //[Required]
    //public string PreviousEmail { get; set; }
}

