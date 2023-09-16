using System;
using Creative.Core.Extention;
using Microsoft.AspNetCore.Identity;

namespace BRMSAPI.Domain;

public class Passengers : IdentityUser
{

	public string FirstName { get; set; }

	public string SurnName { get; set; }

	public string OtherName { get; set; }

    [CheckNumber(0, ErrorMessage = "Service Type is required")]
    public int ServiceType { get; set; }

    [CheckNumber(0, ErrorMessage = "Passenger Type is required")]
    public int PassengerType { get; set; }

	public int AdultCount { get; set; }

	public int ChildrenCount { get; set; }

    [CheckNumber(0, ErrorMessage = "PickUpPoint is required")]
    public int PickUpPointId { get; set; }

	public PickUpPoint PickUpPoint { get; set; }

	[CheckNumber(0, ErrorMessage ="RouteId is required")]
	public  int  RouteId { get; set; }

	public RegRoute Route { get; set; }

    [CheckNumber(0, ErrorMessage = "LCDA is required")]
    public int LCDA { get; set; }

    [CheckNumber(0, ErrorMessage = "Gender is required")]
    public int Gender { get; set; }

	public string EmailToRevalidate { get; set; }

	public string BirthDate { get; set; }

	public string Address { get; set; }

    public string LastActivityDate { get; set; }

    public string LastLoginIpAddress { get; set; }

	public string DateOfBirth { get; set; }

    public string DatedJoined { get; set; }

    public string UpdatedDated { get; set; }

}

