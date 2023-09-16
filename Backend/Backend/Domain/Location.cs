using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BRMSAPI.Utility;
using Configuration;
using Creative.Core.Extention;

namespace BRMSAPI.Domain;

public class Location
{

	public Location()
	{
		PickUpPoints = new HashSet<PickUpPoint>();
    }

	public int LocationId { get; set; }

    [StringLength(50, MinimumLength =3, ErrorMessage ="Title is Required")]
	[Required(AllowEmptyStrings = false)]
    public string Title { get; set; }

    [StringLength(500, MinimumLength = 3, ErrorMessage = "Description is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; }

    public string LandMark { get; set; }

    [CheckNumber(0, ErrorMessage = "City is Required")]
	public int City { get; set; }

    public string CityLabel { get; set; }

    [CheckNumber(0, ErrorMessage = "LCDA is Required")]
    public int LCDA { get; set; }

    [NotMapped]
    public string LCDALabel { get; set; }

    public int ObjectStatusId { get; set; }

    [CheckNumber(0, ErrorMessage = "Area is Required")]
    public int Area { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Area Label is Required")]
    [Required(AllowEmptyStrings = false)]
    public string AreaLabel { get; set; }

    public ICollection<PickUpPoint> PickUpPoints { get; set; }
}


public class LocationResObj
{
    public int LocationId { get; set; }
    public bool IsSuccessful { get; set; }
    public ResponseObj ErrorMessage { get; set; }

}
