using System;
using Backend.Domain.Common;

namespace BRMSAPI.Domain;

public class AssignBusRoute : ISoftDeletedEntity
{
    public int AssignBusRouteId { get; set; }

    public int RegRouteId { get; set; }

    public int ServiceType { get; set; }

    public int TotalPassengerCount { get; set; }

    public int TotalChildrenCount { get; set; }

    public string BussAssigned { get; set; }

    public string DateRegistered { get; set; }
    public bool Deleted { get; set; }
}

