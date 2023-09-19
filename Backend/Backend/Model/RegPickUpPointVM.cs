using System;
using BRMSAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace BRMSAPI.Model;

public class RegPickUpPointVM
{

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    public string Title { get; set; }

    [StringLength(300, MinimumLength = 3, ErrorMessage = "Description is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "BusStop is Required")]
    [Required(AllowEmptyStrings = false)]
    public string BusStop { get; set; }

    public bool Status { get; set; }

    public int ObjectState { get; set; }

    public int StateLevel { get; set; }

    public int LocationId { get; set; }
    public LocationVM Location { get; set; }
    public string UniqueCode { get; set; }


    //public int RegRouteId { get; set; }
    //public RegRoute Route { get; set; }
}

public class PickUpPointVM
{
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name is Required")]
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

    public int ContentStatus { get; set; }

    public string UniqueCode { get; set; }

    public string Date { get; set; }
    public bool IsStartOrEndPoint { get; set; }
    public int LocationId { get; set; }
    public LocationVM Location { get; set; }
    public int RegRouteId { get; set; }
    public RegRoute Route { get; set; }
}


