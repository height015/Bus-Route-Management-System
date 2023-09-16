using System;
using System.ComponentModel.DataAnnotations;

namespace BRMSAPI.Domain;

public class RegRoute
{

    public RegRoute()
    {
        PickUpPoints = new HashSet<PickUpPoint>();
        Passenger = new HashSet<Passengers>();
    }
    public int RegRouteId { get; set; }

    [StringLength(50, MinimumLength =3, ErrorMessage = "Name is Required")]
    [Required(AllowEmptyStrings =false)]

    public string Name { get; set; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Title is Required")]
    [Required(AllowEmptyStrings = false)]

    public string Title { get; set; }

    public int LCDA { get; set; }

    [StringLength(300, MinimumLength = 3, ErrorMessage = "Description is Required")]
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; }

    public int StartPoint { get; set; }

    public int EndPoint { get; set; }

    public int Status { get; set; }

    public string PickUpPointIds { get; set; }

    public ICollection<Bus> Buses { get; set; }

    public ICollection<PickUpPoint> PickUpPoints { get; set; }

    public ICollection<Passengers> Passenger { get; set; }


}

