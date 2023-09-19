using System.Net;
using AutoMapper;
using BRMS.Controllers;
using BRMSAPI.Domain;
using BRMSAPI.Model;
using BRMSAPI.Service;
using BRMSAPI.Utility;
using Configuration;
using Extention;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BRMSAPI.Controllers;

public class PickUpPointController : BaseApiController
{
	private readonly IPickUpPointService _pickUpPointService;
    private readonly ILocationService _locationService;

    public PickUpPointController(IPickUpPointService pickUpPointService, ILocationService locationService,  UserManager<Passengers> userManager, IMapper mapper, IHttpContextAccessor accessor) : base(userManager, mapper, accessor)
    {
        _pickUpPointService = pickUpPointService;
        _locationService = locationService;
    }

    /// <summary>
    /// add Location 
    /// </summary>
    /// <param name="RegPickUpPointVM"></param>
    /// <returns> IsSuccessful = true, Id = id,  Error = "" </returns>
    /// <response code="200">Returns Success</response>
    /// <response code="400">If the object is null</response>
    /// <response code="401">If user is Unauthorized </response>
    [ProducesResponseType(typeof(ResponseResObj), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("add-pick-up-point")]
    public async Task<ActionResult> AddPickUpPoint(RegPickUpPointVM regPickUpPointVM)
    {
        try
        {
            var userData = await CurrentUser();
            var resultLocation = new LocationResObj();
            var modellocation = new Location();
            var locationObj = (await _locationService.GetAllLocation());
           
            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                      $" Your session has expired "));
            }

            if (string.IsNullOrEmpty(regPickUpPointVM.Title) || string.IsNullOrWhiteSpace(regPickUpPointVM.Title))
            {
                regPickUpPointVM.Title = regPickUpPointVM.Name.Trim().ConvertFirstLetterToUpper();
            }

            //if Location Id is Less than 1 then create a new Location
            if (regPickUpPointVM.Location.Id < 1)
            {
                //check if the Location Title Exist
                var isTitleExist = (locationObj).Any(x => x.Title == regPickUpPointVM.Location.Name);

                if (isTitleExist)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                   $"The Location Name {regPickUpPointVM!.Location!.Name}  is already in use"));
                }
                if (regPickUpPointVM.Location.Description.Trim().ContainsOnlySpecialCharacters())
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                  $"Description Contain only contain special character"));
                }
                if (regPickUpPointVM.Location.Landmark.Trim().ContainsOnlySpecialCharacters())
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                $"Landmark Contain only contain special character"));

                }

                //checkk for 
                var isTitleExt = (await _pickUpPointService.GetAllPickUp()).Any(x => x.Name == regPickUpPointVM.Location.Name);

                if (isTitleExt)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                   $"The Name {regPickUpPointVM.Name}  is already in use"));
                }

                modellocation = new Location
                {
                    LocationId = regPickUpPointVM.Location.Id,
                    Title = regPickUpPointVM.Location.Name.Trim().ConvertFirstLetterToUpper(),
                    Description = regPickUpPointVM.Description.Trim().ConvertFirstLetterToUpper(),
                    LandMark = regPickUpPointVM.Location.Landmark.Trim().ConvertFirstLetterToUpper(),
                    City = regPickUpPointVM.Location.City,
                    Area = regPickUpPointVM.Location.Area,
                    LCDALabel = ((LCDAToString)regPickUpPointVM?.Location?.LocalCoucilArea).ToUtilString(),
                    ObjectStatusId = (int)ContentStatus.Authorized,
                };

                if (!modellocation.ObjValid(out var msg1))
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                     $"Error Occured {msg1}"));
                }

                var rnd = new Random();
                var retValCount = ((await _pickUpPointService.GetAllPickUp()).Count()) + (resultLocation.LocationId + 2);


                var pickUp = new PickUpPoint
                {
                    Name = regPickUpPointVM.Name.ConvertFirstLetterToUpper().Trim(),
                    Title = regPickUpPointVM.Title.ConvertFirstLetterToUpper().Trim(),
                    BusStop = regPickUpPointVM.BusStop.ConvertFirstLetterToUpper().Trim(),
                    Description = regPickUpPointVM.Description.ConvertFirstLetterToUpper().Trim(),
                    ObjectState = (int)ContentStatus.Authorized,
                    StateLevel = (int)ObjectState.Available,
                    Location = modellocation,
                    DateCreated = DateTime.Now.ToString("yyyy/MM/dd"),
                    Code = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} - {rnd.Next(999)} - {retValCount} ",
                };

                var resultData = await _pickUpPointService.InsertPickUk(pickUp);

                modellocation.PickUpPoints.Add(pickUp);

                if (resultData.PickUpPointId < 1)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                          $" {resultData.ErrorMessage.ErrorMessage} "));
                }

                resultLocation = await _locationService.InsertLocation(modellocation);

                if (resultLocation.LocationId < 1)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                    $"Unable To create new Location! Error {resultLocation.ErrorMessage}"));
                }

                return Ok(new { IsSuccessful = true, Id = resultData.PickUpPointId, Error = "" });

            }

         

            else if (regPickUpPointVM.Location.Id > 0)
            {
                if (string.IsNullOrEmpty(regPickUpPointVM.Location.Name) || regPickUpPointVM.Location.Name.Length < 3)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                 $"The Location Title {regPickUpPointVM!.Location!.Name}  is already in use"));

                }

                var retLocationObj = await _locationService.GetLocationById(regPickUpPointVM.Location.Id);
                bool areFieldValuesSame = await CompareObjectFields(regPickUpPointVM.Location, retLocationObj);
                modellocation = retLocationObj;
                if (!areFieldValuesSame)  //If the fields values are different create a new Location
                {
                    //clear the object 
                    //modellocation = new Location();
                    //Check if Tite Exist.
                    var isTitleExist = (await _locationService.GetAllLocation()).Any(x => x.Title == regPickUpPointVM.Location.Name);
                    
                    if (isTitleExist)
                    {
                        return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                         $"The Location Title {regPickUpPointVM!.Location!.Name}  is already in use"));
                    }

                    if (regPickUpPointVM.Location.Description.Trim().ContainsOnlySpecialCharacters())
                    {
                        return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                      $"Description Contain only contain special character"));
                    }

                    if (regPickUpPointVM.Location.Landmark.Trim().ContainsOnlySpecialCharacters())
                    {
                        return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                    $"Landmark Contain only contain special character"));
                    }
                     modellocation = new Location
                    {
                        LocationId = regPickUpPointVM.Location.Id,
                        Title = regPickUpPointVM.Location.Name.ConvertFirstLetterToUpper().Trim(),
                        Description = regPickUpPointVM.Description.ConvertFirstLetterToUpper().Trim(),
                        LandMark = regPickUpPointVM.Location.Landmark.ConvertFirstLetterToUpper().Trim(),
                        City = regPickUpPointVM.Location.City,
                        Area = regPickUpPointVM.Location.Area,
                        LCDALabel = ((LCDAToString)regPickUpPointVM?.Location?.LocalCoucilArea).ToUtilString()
                    };
                   
                    modellocation.ObjectStatusId = (int)ContentStatus.Authorized;
                }


                if (!modellocation.ObjValid(out var msg2))
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                  msg2));
                }

                var rnd = new Random();
                var retValCount = ((await _pickUpPointService.GetAllPickUp()).Count()) + (resultLocation.LocationId + 2);


                var pickUp = new PickUpPoint
                {
                    Name = regPickUpPointVM.Name.ConvertFirstLetterToUpper().Trim(),
                    Title = regPickUpPointVM.Title.ConvertFirstLetterToUpper().Trim(),
                    BusStop = regPickUpPointVM.BusStop.ConvertFirstLetterToUpper().Trim(),
                    Description = regPickUpPointVM.Description.ConvertFirstLetterToUpper().Trim(),
                    ObjectState = (int)ContentStatus.Authorized,
                    StateLevel = (int)ObjectState.Available,
                    Location = modellocation,
                    DateCreated = DateTime.Now.ToString("yyyy/MM/dd"),
                    Code = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} - {rnd.Next(999)} - {retValCount} ",
                };

                var resultData = await _pickUpPointService.InsertPickUk(pickUp);

                modellocation.PickUpPoints.Add(pickUp);

                if (resultData.PickUpPointId < 1)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                          $" {resultData.ErrorMessage.ErrorMessage} "));
                }
                resultLocation = await _locationService.InsertLocation(modellocation);


                if (resultLocation.LocationId < 1)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                $"Unable To create new Location! Error {resultLocation.ErrorMessage}"));
                }

                return Ok(new { IsSuccessful = true, Id = resultData.PickUpPointId, Error = "" });
            }

            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
               $"Can not determin object State to process"));

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                    $" Process Error Occurred! Please try again later"));
        }
    }

    /// <summary>
    ///Update Location 
    /// </summary>
    /// <param name="LocationVM"></param>
    /// <returns> IsSuccessful = true, Id = id,  Error = "" </returns>
    /// <response code="200">Returns Success</response>
    /// <response code="400">If the object is null</response>
    /// <response code="401">If user is Unauthorized </response>
    [ProducesResponseType(typeof(ResponseResObj), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("update-location")]
    public async Task<ActionResult> UpdateLocation(RegLocationVM locationVM)
    {
        try
        {
            var userData = await CurrentUser();

            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                     $" Your session has expired "));
            }

            //var model = _mapper.Map<EditLocationObj>(locationVM);

            var model = new Location
            {
                Title = locationVM.Name,
                Description = locationVM.Description,
                LandMark = locationVM.LandMark,
                City = locationVM.City,
                Area = locationVM.Area,
            };

            // model.AdminUserId = userData.UserId;

            model.LCDALabel = ((LCDAToString)model.LCDA).ToUtilString();


            if (!model.ObjValid(out var msg))
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                       $" {msg} "));
            }

            var resultData = await _locationService.UpdateLocation(model);

            if (resultData.LocationId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                     $" {resultData.ErrorMessage.ErrorMessage} "));
            }

            return Ok(new { IsSuccessful = true, Id = resultData.LocationId, Error = "" });

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                    $" Process Error Occurred! Please try again later"));
        }
    }

    /// <summary>
    ///return a list Location 
    /// </summary>
    /// <returns> The list if location</returns>
    /// <response code="200"> Returns the list of location </response>
    /// <response code="204">If result is empty </response>
    /// <response code="401">If user is Unauthorized </response>
    [ProducesResponseType(typeof(List<LocationMod>), 200)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("list-location")]
    public async Task<ActionResult> ListLocation()
    {
        var model = new List<LocationMod>();
        try
        {
            var userData = await CurrentUser();

            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                      $" Your session has expired "));
            }

            var list = (await _locationService.GetAllLocation()).Where(x => x.ObjectStatusId == (int)ContentStatus.Authorized && x.Deleted == false);

            list.ToList();

            if (list == null || !list.Any())
            {
                return NoContent();
            }

            model = list.Select(x => new LocationMod
            {
                Id = x.LocationId,
                Title = x.Title,
                AreaLabel = x.AreaLabel,
                Landmark = x.LandMark,
                LCDA = x.LCDA,
                LCDALabel = x.LCDALabel,
                City = x.City,
                CityLabel = x.CityLabel,
                ObjectStatusId = x.ObjectStatusId,
                Description = x.Description,
                Area = x.Area
            }).ToList();

            return Ok(new { IsSuccessful = true, location = model, Error = "" });

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                    $"Process Error Occurred! Please try again later"));
        }
    }
    /// <summary>
    /// add Location 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns> IsSuccessful = true, Id = id,  Error = "" </returns>
    /// <response code="200">Returns Success</response>
    /// <response code="400">If the object is null</response>
    /// <response code="401">If user is Unauthorized </response>

    [ProducesResponseType(typeof(LocationMod), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("location-detail")]
    public async Task<ActionResult> LocationDetails(int Id)
    {
        try
        {
            var userData = await CurrentUser();

            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                      $" Your session has expired "));
            }

            if (Id <= 0)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, "Invalid Identity"));
            }

            var reVal = (await _locationService.GetLocationById(Id));

            if (reVal == null || reVal.LocationId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, "Invalid or Empty Object"));
            }

            var model = new LocationMod
            {
                Id = reVal.LocationId,
                Title = reVal.Title,
                Description = reVal.Description,
                Landmark = reVal.LandMark,
                City = reVal.City,
                Area = reVal.Area,
                LCDA = reVal.LCDA,
                LCDALabel = reVal.LCDALabel,
                AreaLabel = reVal.AreaLabel,
                CityLabel = reVal.CityLabel,
                ObjectStatusId = reVal.ObjectStatusId
            };

            return Ok(model);

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                    $"Process Error Occurred! Please try again later"));
        }
    }

    [ProducesResponseType(typeof(PickUpPointVM), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("process-update")]
    public async Task<ActionResult> ProcessUpdatePickUpPoint(PickUpPointVM pickPointVM)
    {
        try
        {
            var userData = await CurrentUser();

            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(
                    HttpStatusCode.Unauthorized,
                      $" Your session has expired ")
                    );
            }

            if (pickPointVM.Location.Description
               .ContainsOnlySpecialCharacters())
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                 $"Description only contain special character"));
            }

            if (pickPointVM.Location.Landmark.ContainsOnlySpecialCharacters())
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                $"Landmark only contain special character"));
            }

            if (string.IsNullOrEmpty(pickPointVM.Title) || string.IsNullOrWhiteSpace(pickPointVM.Title))
            {
                pickPointVM.Title = pickPointVM.Name.Trim().ConvertFirstLetterToUpper();
            }

            var pickUp = (await _pickUpPointService.GetPickUpPointById(pickPointVM.Id));

               var location =  new Location
                {
                    LCDA = pickPointVM.Location.LocalCoucilArea,
                    Area = pickPointVM.Location.Area,
                    City = pickPointVM.Location.City,
                    Description = pickPointVM.Location.Description.Trim().ConvertFirstLetterToUpper(),
                    Title = pickPointVM.Location.Name.Trim().ConvertFirstLetterToUpper(),
                    LandMark = pickPointVM.Location.Landmark.Trim().ConvertFirstLetterToUpper(),
                    ObjectStatusId = (int)ContentStatus.Authorized,
                    LocationId = pickPointVM.Location.Id,
                };

            if (!location.ObjValid(out var msg2))
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                               msg2));
            }

            var model = new PickUpPoint
            {
                PickUpPointId = pickPointVM.Id,
                Title = pickPointVM.Title.Trim().ConvertFirstLetterToUpper(),
                LocationId = pickPointVM.LocationId,
                BusStop = pickPointVM.BusStop.Trim().ConvertFirstLetterToUpper(),
                Description = pickPointVM.Description.Trim().ConvertFirstLetterToUpper(),
                Name = pickPointVM.Name.Trim().ConvertFirstLetterToUpper(),
                Code = pickPointVM.UniqueCode,
                StateLevel = (int)ObjectState.Available,
                ObjectState = (int)ContentStatus.Authorized,
                IsStartOrEndPoint = pickPointVM.IsStartOrEndPoint,
                StateLevelLabel = ((ObjectState)(int)ObjectState.Available).ToUtilString(),
                ObjectStateLabel = ((ContentStatus)(int)ContentStatus.Authorized).ToUtilString(),
                DateCreated = pickPointVM.Date,
                RegRouteId = pickUp.RegRouteId,
                Route = pickUp.Route,
                Location = location
            };

            if (model.StateLevel == (int)ObjectState.Suspended)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                  $"You can not Edit a Pick-Up Point that is on Suspension! Please try again later"));
            }

            Location Locat = model.Location;

           

            if (!location.ObjValid(out var msg))
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                    $"Error Occured {msg}"));
            }

            var resultLocation = await _locationService.UpdateLocation(location);

            if (resultLocation.LocationId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
               $"Unable To Update Location! Please try again later"));

            }

            if (!model.ObjValid(out var msg1))
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                    $"Error Occured {msg1}"));
            }

            var resultData = await _pickUpPointService.UpdatePickUp(model);

            if (resultData.PickUpPointId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                   $"Unable To Update Pick-Up Point! Please try again later"));
            }

            return Ok(new { IsSuccessful = true, Id = resultData.PickUpPointId, Error = "" });


        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);

            return BadRequest(new ApiStatusResponse(HttpStatusCode.InternalServerError,
                                  $"Process Error Occurred! Please try again later"));
        }
    }

    [HttpPost("process-bulk-update")]
    public async Task<ActionResult> ProcessUpdateBulkPickUpPoint(List<RegPickUpPointVM> regPickUpPointVMs)
    {
        var resultLocation = new LocationResObj();
        int k = 0;
        var resultData = new PickUpPointResObj();

        if (!regPickUpPointVMs.Any())
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                             $"Process Error Occurred! Empty List "));
        }

        try
        {
            var userData = await CurrentUser();
            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                                                            $"Session has expired"));
            }

            foreach (var x in regPickUpPointVMs)
            {
                k++;
                var locationObj = new LocationVM
                {
                    Name = x.Location.Name.Trim().ConvertFirstLetterToUpper(),
                    Description = x.Location.Description.Trim().ConvertFirstLetterToUpper(),
                    Landmark = x.Location.Landmark.Trim().ConvertFirstLetterToUpper(),
                    City = x.Location.City,
                    LocalCoucilArea = x.Location.LocalCoucilArea,
                    Area = x.Location.Area,
                    Id = x.Location.Id,
                };

                if (locationObj == null)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                                           $"Location is null"));
                }
                
                if (locationObj.Id > 0)
                {

                    var retLocationObj = await _locationService.GetLocationById(locationObj.Id);

                    bool areFieldValuesSame = await CompareObjectFields(locationObj, retLocationObj);

                    if (!areFieldValuesSame)
                    {
                        bool isTitleExist = (await _locationService.GetAllLocation()).Any(x => x.Title == locationObj.Name);

                        if (isTitleExist)
                        {
                            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                                          $"Location Title {locationObj.Name} on Row {k} is already in use"));
                        }

                        var modellocation = _mapper.Map<Location>(locationObj);

                        if (!modellocation.ObjValid(out var msg1))
                        {
                            return BadRequest(new 
                                ApiStatusResponse(HttpStatusCode.BadRequest, msg1));
                        }

                        modellocation.ObjectStatusId = (int)ContentStatus.Authorized;
                        resultLocation = await _locationService.InsertLocation(modellocation);

                        if (resultLocation.LocationId < 1)
                        {
                            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                                         $"Process Error! {resultLocation.ErrorMessage} Could not Create Location "));
                        }

                        locationObj.Id = resultLocation.LocationId;
                        regPickUpPointVMs[k].LocationId = resultLocation.LocationId;
                    }

                }

                else if (locationObj.Id < 1)
                {
                    var checkDuplicateRec = await CompareObjectFields(locationObj);

                    if (!checkDuplicateRec)
                    {
                        var modellocation = _mapper.Map<Location>(locationObj);

                        if (!modellocation.ObjValid(out var msg1))
                        {
                            return BadRequest(new
                                ApiStatusResponse(HttpStatusCode.BadRequest, msg1));
                        }

                        modellocation.ObjectStatusId = (int)ContentStatus.Authorized;

                        resultLocation = await _locationService.InsertLocation(modellocation);

                        if (resultLocation.LocationId < 1)
                        {
                            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                                                                    $"Process Error! {resultLocation.ErrorMessage} Could not Create Location "));
                        }
                    }

                }

                var rnd = new Random();
                var pickUpPoints = (await _pickUpPointService.GetAllPickUp()).Count();
                var retValCount = pickUpPoints + (resultLocation.LocationId) + 2;

                var pickup = new RegPickUpPointVM
                {
                    Name = x.Name.Trim().ConvertFirstLetterToUpper(),
                    Title = x.Title.Trim().ConvertFirstLetterToUpper(),
                    Description = x.Description.Trim().ConvertFirstLetterToUpper(),
                    BusStop = x.BusStop.Trim().ConvertFirstLetterToUpper(),
                    Location = locationObj,
                    LocationId = resultLocation.LocationId,
                    ObjectState = (int)ContentStatus.Authorized,
                    StateLevel = (int)ObjectState.Available,
                    UniqueCode = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} - {rnd.Next(999)} - {retValCount} ",

                };

                pickup.LocationId = resultLocation.LocationId;

                var model = _mapper.Map<PickUpPoint>(pickup);
                model.DateCreated = DateTime.UtcNow.ToString("MM/dd/yyyy");
                model.ObjectState = (int)ContentStatus.Authorized;
                //model.Location = modellocation,

                if (!model.ObjValid(out var msg))
                {
                    return BadRequest(new 
                        ApiStatusResponse(HttpStatusCode.BadRequest, msg));
                }

                 resultData = await _pickUpPointService.InsertPickUk(model);

                if (resultData.PickUpPointId < 1)
                {
                    return BadRequest(new
                        ApiStatusResponse(HttpStatusCode.BadRequest,
                        $"Process Error! {resultLocation.ErrorMessage} Could not Create Pick-up point "));

                }
            }




            return Ok(new { IsAuthenticated = true, Id = resultData.PickUpPointId, IsSuccessful = true,  Error = string.Empty });


        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new
                                  ApiStatusResponse(HttpStatusCode.InternalServerError));
        }
    }

    [HttpPost("delete-item")]
    public async Task<ActionResult> ProcessDeletePickUp(int id)
    {
        try
        {
            //int rowIndex = id - 1;
            var userData = await CurrentUser();
            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                                                            $"Session has expired"));
            }
           
            var retVal = await _pickUpPointService.GetPickUpPointById(id);

            if (retVal == null || retVal.Name.Length < 2)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                             $"Couldn't Complete Delete, Pickup Point Not Found"));
            }

            if (retVal == null || retVal.Name.Length < 2)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                            $"Couldn't Complete Delete, Pickup Point Not Found"));

            }

            if (retVal.Route != null || retVal.RegRouteId > 0)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                           $"Couldn't Complete Delete, Pickup is mapped to Route"));
            }

            if (retVal.Route != null)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                          $"Couldn't Complete Delete, Pickup Point is already Mapped to Route"));
            }

            var response = await _pickUpPointService.DeletePickUpPoint(id);

            if (!response.IsSuccessful)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                         $"Unable to delete PickUp Point")); 
            }

            return Ok(new { IsAuthenticated = true, IsSuccessful = true, Error = "" });
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.InternalServerError,
                                         $"Proccess Error"));
        }
    }

    [HttpPost("suspend-item")]
    public async Task<ActionResult> ProcessSuspendPickUp(int id)
    {
        try
        {
            var userData = await CurrentUser();
            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                                                            $"Session has expired"));
            }

            var retVal = await _pickUpPointService.GetPickUpPointById(id);

            if (retVal == null || retVal.Name.Length < 2)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                               $"Couldn't Complete Suspend Action, Pickup Point Not Found"));
            }

            if (retVal == null || retVal.Name.Length < 2)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                              $"Couldn't Complete Suspend Action, Pickup Point Not Found"));
            }

            if (retVal.IsStartOrEndPoint)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                             $"You can not Suspend a Start or End Pick-Up Point"));
            }

            var model = new PickUpPoint
            {
                PickUpPointId = retVal.PickUpPointId,
                Title = retVal.Title.Trim().ConvertFirstLetterToUpper(),
                LocationId = retVal.LocationId,
                BusStop = retVal.BusStop.Trim().ConvertFirstLetterToUpper(),
                Description = retVal.Description.Trim().ConvertFirstLetterToUpper(),
                Name = retVal.Name.Trim().ConvertFirstLetterToUpper(),
                Code = retVal.Code,
                Route = retVal.Route,
                RegRouteId = retVal.RegRouteId,
                ObjectState = (int)ContentStatus.Authorized,
                StateLevel = (int)ObjectState.Suspended,
                Location = retVal.Location,
                DateCreated = retVal.DateCreated,
            };
            if (!model.ObjValid(out var msg))
            {
                return BadRequest(new
                                      ApiStatusResponse(HttpStatusCode.BadRequest, msg));
            }

            var resultData = await _pickUpPointService.UpdatePickUp(model);


            if (resultData.PickUpPointId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                                            $"Failed to create Pick-Up Point"));
            }

            return Ok(new { IsAuthenticated = true, IsSuccessful = true, Error = "" });

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.InternalServerError));
        }
    }

    [HttpPost("restore-item")]
    public async Task<ActionResult> ProcessRestorePickUp(int id)
    {
        try
        {
            var userData = await CurrentUser();
            if (string.IsNullOrEmpty(userData.UserId))
            {
                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                                                            $"Session has expired"));
            }
            var retVal = await _pickUpPointService.GetPickUpPointById(id);
            if (retVal == null || retVal.Name.Length < 2)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                             $"Couldn't Complete Restore Action, Pickup Point Not Found"));
            }

            if (retVal == null || retVal.Name.Length < 2)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                             $"Couldn't Complete Restore, Pickup Point Not Found"));
            }

            var model = new PickUpPoint
            {
                PickUpPointId = retVal.PickUpPointId,
                Title = retVal.Title.Trim().ConvertFirstLetterToUpper(),
                LocationId = retVal.LocationId,
                BusStop = retVal.BusStop.Trim().ConvertFirstLetterToUpper(),
                Description = retVal.Description.Trim().ConvertFirstLetterToUpper(),
                Name = retVal.Name.Trim().ConvertFirstLetterToUpper(),
                Code = retVal.Code,
                Route = retVal.Route,
                RegRouteId = retVal.RegRouteId,
                ObjectState = (int)ContentStatus.Authorized,
                StateLevel = (int)ObjectState.Available,
                Location = retVal.Location,
                DateCreated = retVal.DateCreated,
            };

            if (!model.ObjValid(out var msg))
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));
            }

            var resultData = await _pickUpPointService.UpdatePickUp(model);

            if (resultData.PickUpPointId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                                                            $"Couldn't Complete Restore, Pickup Point Not Found"));
            }

            return Ok(new { IsAuthenticated = true, Id = resultData.PickUpPointId,  IsSuccessful = true, Error = "" });

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.InternalServerError,
                                                                      $"Process Error"));
        }
    }

    #region Utility
    private async Task<bool> CompareObjectFields(LocationVM currentObject, Location originalObject = null)
    {
        if (originalObject != null)
        {
            if (

                currentObject.Name.Trim().ToLower() != originalObject.Title.Trim().ToLower() ||
                currentObject.Landmark.Trim().ToLower() != originalObject.LandMark.Trim().ToLower()
                || currentObject.Description.Trim().ToLower() != originalObject.Description.Trim().ToLower()
                || currentObject.City != originalObject.City
                || currentObject.Area != originalObject.Area
                || currentObject.LocalCoucilArea != originalObject.LCDA)
            {
                return false;
            }
        }
        else
        {
            var checkDuplicate = (await _locationService.GetAllLocation()).Any(x => x.Title == currentObject.Name);

            if (checkDuplicate)
            {
                return true;
            }
            return false;
        }

        return true;
    }

    private async Task<List<PickUpPoint>> FetchPickUpListByIds(int[] Ids)
    {

        var pickObj = new List<PickUpPoint>();
        try
        {
            if (Ids.Length < 1)
            {
                return pickObj;            
            }

            var retVal = (await _pickUpPointService.GetAllPickUp()).Where(x => Ids.Contains(x.PickUpPointId));
            if (retVal.Any())
            {
                return retVal.ToList();
            }

            return pickObj;

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);

            return pickObj;
        }
    }

    #endregion

}

