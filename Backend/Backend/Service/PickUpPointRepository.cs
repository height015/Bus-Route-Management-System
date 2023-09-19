using System;
using BRMSAPI.Data;
using BRMSAPI.Domain;
using Configuration;
using Data.Repository;
using Extention;

namespace BRMSAPI.Service;

public class PickUpPointRepository : IPickUpPointService
{
    private readonly IRepository<PickUpPoint> _pickUpPointRepository;
    private ResponseObj _errorObj;

    protected CancellationTokenSource _cancellationTokenSource ;


    public PickUpPointRepository(AppDbContext appDbContext)
    {
        _pickUpPointRepository = new Repository<PickUpPoint>(appDbContext);
        _errorObj = new ResponseObj();
    }
    public async Task<PickUpPointResObj> DeletePickUpPoint(int pickUpId)
    {
        var response = new PickUpPointResObj
        {
            ErrorMessage = new ResponseObj(),
            IsSuccessful = false,
            PickUpPointId = -1,
        };

        try
        {
            if (pickUpId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! invalid identity";
                _errorObj.TechMessage = "Records could not be fetch because " +
                    "Identity is less than or equals zero";
                response.ErrorMessage = _errorObj;

                return response;
            }

            var pickupPoint = await  _pickUpPointRepository.DeleteAsync(pickUpId);

            if (!string.IsNullOrEmpty(pickupPoint))
            {
                _errorObj.ErrorMessage = "Error Occurred! Could not Complete Process";
                _errorObj.TechMessage = "Error Occurred! Could not Complete Process";
                response.ErrorMessage = _errorObj;
                return response;
            }
             response.IsSuccessful = true;

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

    public async Task<IQueryable<PickUpPoint>> GetAllPickUp()
    {
        try
        {
            return (await _pickUpPointRepository.Fetch());
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new List<PickUpPoint>().AsQueryable();
        }
    }

    public async Task<PickUpPoint> GetPickUpPointById(int pickUpId)
    {
        try
        {
            if (pickUpId <= 0)
            {
                return new PickUpPoint();
            }

            return await _pickUpPointRepository.getById(pickUpId);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new PickUpPoint();
        }
    }

    public async Task<PickUpPointResObj> InsertPickUk(PickUpPoint pickUpPoint)
    {

        var response = new PickUpPointResObj
        {
            IsSuccessful = false,
            PickUpPointId = -1,
            ErrorMessage = new ResponseObj(),
        };

        
        try
        {

            if (pickUpPoint == null)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Error Occurred! Please try again later Object is null";
                response.ErrorMessage = _errorObj;
                return response;
            }


            //if (!pickUpPoint.ObjValid(out var msg))
            //{
            //    _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
            //    response.ErrorMessage = _errorObj;
            //    return response;
            //}


            var retVal = await _pickUpPointRepository.Insert(pickUpPoint);

            if (retVal == null || retVal.PickUpPointId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.PickUpPointId = retVal.PickUpPointId;
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

    public async Task<PickUpPointResObj> UpdatePickUp(PickUpPoint pickUpPoint)
    {
        var response = new PickUpPointResObj
        {
            IsSuccessful = false,
            PickUpPointId = -1,
            ErrorMessage = new ResponseObj()
        };

        try
        {
            if (pickUpPoint == null || pickUpPoint.PickUpPointId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Error Occurred! Please try again later Object is null";
                response.ErrorMessage = _errorObj;
                return response;
            }

            if (!pickUpPoint.ObjValid(out var msg))
            {
                _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
                response.ErrorMessage = _errorObj;
                return response;
            }

            var retVal = await _pickUpPointRepository.Update(pickUpPoint);

            if (retVal == null || retVal.PickUpPointId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.PickUpPointId = retVal.PickUpPointId;
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

