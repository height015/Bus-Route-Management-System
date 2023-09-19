
using BRMSAPI.Domain;
namespace BRMSAPI.Service;

public interface IScheduleServices
{
    Task<ScheduleRespObj> DeleteSchedule(int taskId);

   
    Task<Schedule> GetScheduleId(int taskId);

  
    Task<Schedule> GetScheduleByName(string type);

   
    Task<IQueryable<Schedule>> GetAllTasks();

    
    Task<ScheduleRespObj> InsertTask(Schedule task);

   
    Task<ScheduleRespObj> UpdateTask(Schedule task);
}

