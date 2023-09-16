using System;
namespace BRMSAPI.Utility;

public enum ObjectState
{
    Available=1,
    Suspended,
    Fresh
}

public enum PickUpStatus
{
    Queried = -1, //Have issues with authorization process
    Unknown = 0,
    Fresh = 1,
    Pending_Request,
    Modification, //Authorized for Modification
    Modified,
    Reviewed,
    Authorized,
    Published,
    Archived,
}

public enum ContentStatus
{
    Queried = -1,
    Fresh = 1,
    Pending_Request,
    Modification,
    Modified,
    Reviewed,
    Authorized,
    Published,
    Archived,
}
public enum ContentType
{
    Pick_Up_Poin = 6
}

public enum Status
{
    In_Active = 0,
    Active
}

public enum ServiceType
{
    First_Service = 1,
    Second_Service,
}
public enum ServiceTypeLab
{
    First_Service = 1,
    Second_Service,
    All_Service
}

public enum LCDAToString
{
    Igbogbo_Baiyeku = 1,
    Ikorodu_North,
    Ikorodu_West,
    Imota,
    Ijede,
}

public enum ObjState
{
    Available = 1,
    Suspended,
    Active
}

public enum Gender
{
    Male = 1,
    Female
}

public enum PassengerType
{
    Single = 1,
    Family,
    Group
}

public enum BusTypeEnu
{
    Coaster_Bus = 1,
    Multi_Purpose_Bus,
    Small_Passenger_Bus
}

public enum BusManufactural
{
    Toyota = 1,
    Ford,
    Honda,
    Volkswagen,
    Mitsubishi,
    Mazda,
    Hyundai,
    Nissan,
    BMW,
    Chevrolet,
    Audi,
    Tesla,
    Volvo,
    Mercedes_Benz
}
public enum BusColor
{
    Black = 1,
    White,
    Yellow,
    Silver,
    Green,
    Blue,
    Gray,
    Red,
    Brown
}
public enum BusFuelType
{
    Petrol = 1,
    Diesel,
    Electric
}
