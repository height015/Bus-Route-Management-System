using System.ComponentModel.DataAnnotations;
using Backend.Domain.Common;
using BRMSAPI.Model;
using Configuration;

namespace BRMSAPI.Domain;


public class PickUpPoint : ISoftDeletedEntity
{
	public int PickUpPointId { get; set; }

	[StringLength(50, MinimumLength =3, ErrorMessage = "Name is Required")]
	[Required(AllowEmptyStrings = false)]
	public string Name { get; set; }
	public string Title { get; set; }
    [StringLength(300, MinimumLength = 3, ErrorMessage = "Description is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; }

	[StringLength(50, MinimumLength = 3, ErrorMessage = "BusStop is Required")]
    [Required(AllowEmptyStrings = false)]
    public string BusStop { get; set; }

	public int ObjectState { get; set; }

	public int StateLevel { get; set; }

	public string DateCreated { get; set; }

    public string Code { get; set; }

    public int LocationId { get; set; }
	public Location Location { get; set; }

	public int RegRouteId { get; set; }
	public RegRoute Route { get; set; }

    public bool Deleted { get; set; }

    public bool IsStartOrEndPoint { get; set; }

    public string StateLevelLabel { get; set; }

    public string ObjectStateLabel { get; set; }


}

public class PickUpPointResObj
{
	public int PickUpPointId { get; set; }
	public bool IsSuccessful { get; set; }
	public ResponseObj ErrorMessage { get; set; }
}


