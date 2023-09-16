using System;
using BRMSAPI.Data;
using BRMSAPI.Domain;
using Configuration;
using Data.Repository;
using Extention;

namespace BRMSAPI.Service;

public class BusRepository : IBusService
{
    private readonly IRepository<Bus> _busRepository;
    private ResponseObj _errorObj;
    public BusRepository(AppDbContext appDbContext)
	{
        _busRepository = new Repository<Bus>(appDbContext);
        _errorObj = new ResponseObj();
    }

    public async Task<BusResObj> CreateBus(Bus bus)
    {
        var response = new BusResObj
        {
            BusId = -1,
            IsSuccessful = false,
            ErrorMessage = new ResponseObj()
        };

        try
        {
            if (bus == null)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Error Occurred! Please try again later Object is null";
                response.ErrorMessage = _errorObj;
                return response;
            }

            if (!bus.ObjValid(out var msg))
            {
                _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
                response.ErrorMessage = _errorObj;
                return response;
            }

            var retVal = await _busRepository.Insert(bus);

            if (retVal == null || retVal.BusId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.BusId = retVal.BusId;
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

    public async Task<BusResObj> DeleteBus(int busId)
    {
        var response = new BusResObj
        {
            BusId = -1,
            IsSuccessful = false,
            ErrorMessage = new ResponseObj()
        };

        try
        {
            if (busId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! Invalid Identity";
                _errorObj.TechMessage = "Error Occurred! Please try again later Object is null";
                response.ErrorMessage = _errorObj;
                return response;
            }


            if (busId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! invalid identity";
                _errorObj.TechMessage = "Records could not be fetch because " +
                    "Identity is less than or equals zero";
                response.ErrorMessage = _errorObj;

                return response;
            }

            var busObj = _busRepository.Delete(busId);

            if (busObj == null || string.IsNullOrEmpty(busObj))
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
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            _errorObj.ErrorMessage = "Service Respose Error! Please Try again";
            _errorObj.TechMessage = $"{ex.GetBaseException().Message}";
            response.ErrorMessage = _errorObj;
            return response;
        }

    }

    public async Task<List<Bus>> GetAllBuses()
    {
        try
        {
            return (await _busRepository.Fetch()).ToList();
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new List<Bus>();
        }


    }

    public async Task<Bus> GetBusById(int busId)
    {
        try
        {
            if (busId <= 0)
            {
                return new Bus();
            }

            return await _busRepository.getById(busId);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new Bus();
        }
    }

    public async Task<BusResObj> UpdateBus(Bus bus)
    {
        var response = new BusResObj
        {
            IsSuccessful = false,
            BusId = -1,
            ErrorMessage = new ResponseObj()
        };

        try
        {
            if (bus == null || bus.BusId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Error Occurred! Please try again later Object is null";
                response.ErrorMessage = _errorObj;
                return response;
            }

            if (!bus.ObjValid(out var msg))
            {
                _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
                response.ErrorMessage = _errorObj;
                return response;
            }

            var retVal = await _busRepository.Update(bus);

            if (retVal == null || retVal.BusId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.BusId = retVal.BusId;
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

