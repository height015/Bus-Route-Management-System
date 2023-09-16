using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class LocationVM 
{
    public int Id { get; set; }
    [StringLength(60, MinimumLength = 3, ErrorMessage = "Location Title contains fewer or more characters than expected. (3 to 60 characters are expected)")]
    [Required(AllowEmptyStrings = false)]

    public string LocationTitle { get; set; }

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Description contains fewer or more characters than expected. (5 to 300 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string LocationDescription { get; set; }

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Landmark contains fewer or more characters than expected. (5 to 300 characters are expected)")]
    [Required(AllowEmptyStrings = true)]
    public string Landmark { get; set; }

    [StringLength(15, MinimumLength = 3, ErrorMessage = "City contains fewer or more characters than expected. (5 to 300 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string City { get; set; }

    [StringLength(15, MinimumLength = 3, ErrorMessage = "LCDA contains fewer or more characters than expected. (5 to 300 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string LCDA { get; set; }

    [StringLength(15, MinimumLength = 3, ErrorMessage = "Area contains fewer or more characters than expected. (5 to 300 characters are expected)")]
    [Required(AllowEmptyStrings = false)]
    public string Area { get; set; }


    [Required(AllowEmptyStrings = false)]
    public string PhoneNumber { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string EmailAddress { get; set; }

}
