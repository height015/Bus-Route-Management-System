using System;
using BRMSAPI.Data;
using BRMSAPI.Domain;
using Configuration;
using Data.Repository;
using Extention;

namespace BRMSAPI.Service;

public class LocationRepository : ILocationService
{
    private readonly IRepository<Location> _locationRepository;
    private ResponseObj _errorObj;

	public LocationRepository(AppDbContext appDbContext)
	{
        _locationRepository = new Repository<Location>(appDbContext);
        _errorObj = new ResponseObj();
    }

    public async Task<LocationResObj> DeleteLocation(int locId)
    {
        var response = new LocationResObj
        {
            LocationId = -1,
            IsSuccessful = false,
            ErrorMessage = new ResponseObj()
        };

        try
        {
            if (locId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! invalid identity";
                _errorObj.TechMessage = "Records could not be fetch because " +
                    "Identity is less than or equals zero";
                response.ErrorMessage = _errorObj;
                return response;

            }

            var locatin = await _locationRepository.DeleteAsync(locId);

            if (locatin == null || string.IsNullOrEmpty(locatin))
            {
                _errorObj.ErrorMessage = "Error Occurred! Could not Complete Process";
                _errorObj.TechMessage = "Error Occurred! Could not Complete Process";
                response.ErrorMessage = _errorObj;
                return response;
            }

            return await Task.FromResult(response);

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            _errorObj.ErrorMessage = "Service Respose Error! Please Try again";
            _errorObj.TechMessage = $"{ex.GetBaseException().Message}";
            response.ErrorMessage = _errorObj;
            return response;

        }


    }

    public async Task<IQueryable<Location>> GetAllLocation()
    {
        try
        {
            return (await _locationRepository.Fetch());
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new List<Location>().AsQueryable();
        }
    }

    public async Task<Location> GetLocationById(int locationId)
    {
        try
        {
            if (locationId <= 0)
            {
                return new Location();
            }

            return await _locationRepository.getById(locationId);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new Location();
        }
    }

    public async  Task<LocationResObj> InsertLocation(Location location)
    {
        var response = new LocationResObj
        {
            LocationId = -1,
            IsSuccessful = false,
            ErrorMessage = new ResponseObj()
        };

        try
        {
            if (location == null)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Error Occurred! Please try again later Object is null";
                response.ErrorMessage = _errorObj;
                return response;
            }

            if (!location.ObjValid(out var msg))
            {
                _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
                response.ErrorMessage = _errorObj;
                return response;
            }

            var retVal = await _locationRepository.Insert(location);

            if (retVal == null || retVal.LocationId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.LocationId = retVal.LocationId;
            return response;
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            _errorObj.ErrorMessage = "Service Respose Error! Please Try again";
            _errorObj.TechMessage = $"{ex.GetBaseException().Message}";
            response.ErrorMessage = _errorObj;
            return response;
        }
    }

    public async Task<LocationResObj> UpdateLocation(Location location)
    {
        var response = new LocationResObj
        {
            LocationId = -1,
            IsSuccessful = false,
            ErrorMessage = new ResponseObj()
        };

        try
        {
            if (location == null || location.LocationId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Error Occurred! Please try again later Object is null";
                response.ErrorMessage = _errorObj;
                return response;
            }

            if (!location.ObjValid(out var msg))
            {
                _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
                response.ErrorMessage = _errorObj;
                return response;
            }

            var retVal = await _locationRepository.Update(location);

            if (retVal == null || retVal.LocationId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.LocationId = retVal.LocationId;
            return response;

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            _errorObj.ErrorMessage = "Service Respose Error! Please Try again";
            _errorObj.TechMessage = $"{ex.GetBaseException().Message}";
            response.ErrorMessage = _errorObj;
            return response;
        }
    }
}

