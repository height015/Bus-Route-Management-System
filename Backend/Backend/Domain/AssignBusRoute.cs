using System;
namespace BRMSAPI.Domain;

public class AssignBusRoute
{
    public int AssignBusRouteId { get; set; }

    public int RegRouteId { get; set; }

    public int ServiceType { get; set; }

    public int TotalPassengerCount { get; set; }

    public int TotalChildrenCount { get; set; }

    public string BussAssigned { get; set; }

    public string DateRegistered { get; set; }
}

