using System;
using Backend.Domain.Common;
using Configuration;

namespace BRMSAPI.Domain;

public class Schedule : ISoftDeletedEntity
{

    public int ScheduleId { get; set; }

    public int RouteId { get; set; }

    public RegRoute Route { get; set; }

    public PickUpPoint PickUpPoint { get; set; }

    public int PickPointId { get; set; }

    public int Status { get; set; }

    public int Service { get; set; }

    public string ArrivalTime { get; set; }

    public string DepertureTime { get; set; }

    public bool Deleted { get; set; }

}

public class ScheduleRespObj
{
    public int ScheduleId { get; set; }

    public ResponseObj ErrorMessage { get; set; }

    public bool IsSuccessful { get; set; }

}