using BRMSAPI.Utility;
using Configuration;
using Creative.Core.Extention;
using System.ComponentModel.DataAnnotations;
namespace BRMSAPI.Model;

public class RegLocationVM
{

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    [StringLength(500, MinimumLength = 3, ErrorMessage = "Description is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; }

    public string LandMark { get; set; }

    [CheckNumber(0, ErrorMessage = "City is Required")]
    public int City { get; set; }

    public string CityLabel { get; set; }

    [CheckNumber(0, ErrorMessage = "LCDA is Required")]
    public int LocalCoucilArea { get; set; }

    [CheckNumber(0, ErrorMessage = "Area is Required")]
    public int Area { get; set; }

    //[StringLength(50, MinimumLength = 3, ErrorMessage = "Area Label is Required")]
    //[Required(AllowEmptyStrings = false)]
    //public string AreaLabel { get; set; }

}



public class LocationVM
{
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    [StringLength(500, MinimumLength = 3, ErrorMessage = "Description is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; }

    public string Landmark { get; set; }

    [CheckNumber(0, ErrorMessage = "City is Required")]
    public int City { get; set; }


    [CheckNumber(0, ErrorMessage = "LCDA is Required")]
    public int LocalCoucilArea { get; set; }

    [CheckNumber(0, ErrorMessage = "Area is Required")]
    public int Area { get; set; }

   

}


public class LocationMod
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }

    public string Landmark { get; set; }

    public int City { get; set; }

    public string CityLabel { get; set; }

    public int LCDA { get; set; }

    public string LCDALabel { get; set; }

    public int ObjectStatusId { get; set; }

    public string LocationStateLabel {
        get
        {
            if (ObjectStatusId > 0)
            {
                return ((ObjState)ObjectStatusId).ToUtilString();
            }
            return "";
        }
    }

    public int Area { get; set; }

    public string AreaLabel { get; set; }

}


