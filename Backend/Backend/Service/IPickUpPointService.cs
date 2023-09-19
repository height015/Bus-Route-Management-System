using System;
using BRMSAPI.Domain;

namespace BRMSAPI.Service;

public interface IPickUpPointService
{

    Task<PickUpPoint> GetPickUpPointById(int pickUpId);


    Task<PickUpPointResObj> DeletePickUpPoint(int pickUpId);



    Task<IQueryable<PickUpPoint>> GetAllPickUp();


    Task<PickUpPointResObj> InsertPickUk(PickUpPoint pickUpPoint);
    


    Task<PickUpPointResObj> UpdatePickUp(PickUpPoint pickUpPoint);
}

