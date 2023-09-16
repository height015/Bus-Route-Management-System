using System;
using BRMSAPI.Domain;

namespace BRMSAPI.Service;

public interface ILocationService
{

    Task<Location> GetLocationById(int locationId);

    Task<LocationResObj> DeleteLocation(int pickUpId);

    Task<List<Location>> GetAllLocation();


    Task<LocationResObj> InsertLocation(Location location);

    Task<LocationResObj> UpdateLocation(Location location);
}

