using System;
using System.Reflection;
using Backend.Domain.Common;

namespace BRMSAPI.Domain;

public class PassengerReport : ISoftDeletedEntity
{
    public int PassengerReportId { get; set; }

    public int BusId { get; set; }
    public string RouteName { get; set; }
    public int RouteId { get; set; }
    public int PassengerCount { get; set; }
    public string PhoneNumber { get; set; }
    public string SurnName { get; set; }
    public string OtherName { get; set; }
    public string EmailAddress { get; set; }
    public string UserName { get; set; }
    public int LCDA { get; set; }
    public int PickUpId { get; set; }
    public string PickUpName { get; set; }
    public int ServiceType { get; set; }
    public int AdultCount { get; set; }
    public int ChildrenCount { get; set; }
    public string DateRegistered { get; set; }
    public int PassengerState { get; set; }
    public int TotalPassenger { get; set; }
    public int Gender { get; set; }

    public int PassengerType { get; set; }

    public string PassengerTypeLabel
    { get; set; }

    public string GenderLabel
    { get; set; }

    public string ServiceTypeLabel
    { get; set; }

    public bool Deleted { get; set; }

}

