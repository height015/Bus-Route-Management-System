using System;
using BRMSAPI.Domain;

namespace BRMSAPI.Service;

public interface IBusService
{
    Task<Bus> GetBusById(int busId);

    Task<IQueryable<Bus>> GetAllBuses();

    Task<BusResObj> CreateBus(Bus bus);

    Task<BusResObj> UpdateBus(Bus bus);

    Task<BusResObj> DeleteBus(int busId);




}

