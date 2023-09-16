using System;
using BRMSAPI.Data;
using BRMSAPI.Domain;
using Configuration;
using Data.Repository;

namespace BRMSAPI.Service;

public class ScheduleRepository : IScheduleServices
{
    private readonly IRepository<Schedule> _scheduleRepository;
    private ResponseObj _errorObj;

    public ScheduleRepository(AppDbContext appDbContext)
    {
        _scheduleRepository = new Repository<Schedule>(appDbContext);
        _errorObj = new ResponseObj();
    }

    public async Task<ScheduleRespObj> DeleteSchedule(int scheduleId)
    {
        var response = new ScheduleRespObj
        {
            IsSuccessful = false,
            ScheduleId = 0,
            ErrorMessage = new ResponseObj()
        };



        try
        {
            if (scheduleId <= 0)
            {
                _errorObj.ErrorMessage = "Error Occurred! invalid identity";
                _errorObj.TechMessage = "Records could not be fetch because " +
                    "Identity is less than or equals zero";
                response.ErrorMessage = _errorObj;
            }

            var retVal = _scheduleRepository.Delete(scheduleId);

            if (!string.IsNullOrEmpty(retVal))
            {
                _errorObj.ErrorMessage = "Error Occurred! Could not Delete";
                _errorObj.TechMessage = "Records could not be Deleted recods ";
                response.ErrorMessage = _errorObj;
            }

            response.IsSuccessful = true;
            return await Task.FromResult(response);
        }
        catch (Exception ex)
        {
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            response.ErrorMessage = _errorObj;
            return response;
        }


    }

    public async Task<IList<Schedule>> GetAllTasks()
    {
        try
        {
            return (await _scheduleRepository.Fetch()).ToList();
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new List<Schedule>();
        }
    }

    public async Task<Schedule> GetScheduleByName(string type)
    {
        var response = new ScheduleRespObj
        {
            ScheduleId = -1,
            IsSuccessful = false,
            ErrorMessage = new ResponseObj()
        };


        try
        {
            if (type == string.Empty || type == null)
            {
                _errorObj.ErrorMessage = "";
                _errorObj.TechMessage = "";
                response.ErrorMessage = _errorObj;

                return new Schedule();
            }
            var retVal = _scheduleRepository.Table.FirstOrDefault(x => x.Route.Name == type, new Schedule());

            return await Task.FromResult(retVal);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new Schedule();
        }
    }

    public async Task<Schedule> GetScheduleId(int taskId)
    {
        var retVal = new Schedule();
        try
        {
            retVal = await _scheduleRepository.getById(taskId);
            return retVal;
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return retVal;
        }
    }

    public async Task<ScheduleRespObj> InsertTask(Schedule schedule)
    {
        var response = new ScheduleRespObj
        {
            ScheduleId = -1,
            IsSuccessful = false,
            ErrorMessage = new ResponseObj(),
        };


        try
        {
            if (schedule == null)
            {
                _errorObj.ErrorMessage = "";
                _errorObj.TechMessage = "";
                response.ErrorMessage = _errorObj;
            }

            //if (!schedule.ObjValid(out var msg))
            //{
            //    _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
            //    response.ErrorMessage = _errorObj;
            //    return response;
            //}


            var retVal = await _scheduleRepository.Insert(schedule);

            if (retVal == null || retVal.ScheduleId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.ScheduleId = retVal.ScheduleId;
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

    public async Task<ScheduleRespObj> UpdateTask(Schedule schedule)
    {
        var response = new ScheduleRespObj
        {
            ErrorMessage = new ResponseObj(),
            IsSuccessful = false,
            ScheduleId = -1,
        };

        try
        {

            if (schedule == null)
            {
                _errorObj.ErrorMessage = "";
                _errorObj.TechMessage = "";
                response.ErrorMessage = _errorObj;
                return response;

            }

            //if (!schedule.ObjectVal(out var msg))
            //{
            //    _errorObj.ErrorMessage = $"Validation Error Occured! Details: {msg}";
            //       response.ErrorMessage = _errorObj;
            //      return response;
            //}


            var retVal = await _scheduleRepository.Update(schedule);

            if (retVal == null || retVal.ScheduleId < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorMessage = _errorObj;
                return response;
            }

            response.IsSuccessful = true;
            response.ScheduleId = retVal.ScheduleId;
            return response;
        }
        catch (Exception ex)
        {
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            response.ErrorMessage = _errorObj;
            return response;

        }

    }
}

