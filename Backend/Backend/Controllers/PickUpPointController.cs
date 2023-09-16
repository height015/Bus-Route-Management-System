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
using Microsoft.IdentityModel.Tokens;

namespace BRMSAPI.Controllers;

public class PickUpPointController : BaseApiController
{
	private readonly IPickUpPointService _pickUpPointService;
    private readonly ILocationService _locationService;

    public PickUpPointController(IPickUpPointService pickUpPointService, ILocationService locationService, UserManager<Passengers> userManager, IMapper mapper) : base(userManager, mapper)
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


            var locationObj = (await _locationService.GetAllLocation());
            if (string.IsNullOrEmpty(userData.UserId))
            {

                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                      $" Your session has expired "));
            }


            //if Location Id is Less than 1 then create a new Location
            if (regPickUpPointVM.Location.Id < 1)
            {
                //check if the Location Title Exist
                var isTitleExist = (locationObj).Any(x => x.Title == regPickUpPointVM?.Location.Name);

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

                //LocationVM Locat = regPickUpPointVM.Location;

                //var modellocation = _mapper.Map<RegLocationObj>(Locat);

                var modellocation = new Location
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


                var resultLocation = await _locationService.InsertLocation(modellocation);

                if (resultLocation.LocationId < 1)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                    $"Unable To create new Location! Error {resultLocation.ErrorMessage}"));
                }

                //pickPointVM.LocationId = resultLocation;

            }

            if (string.IsNullOrEmpty(regPickUpPointVM.Title) || string.IsNullOrWhiteSpace(regPickUpPointVM.Title))
            {
                regPickUpPointVM.Title = regPickUpPointVM.Name.Trim().ConvertFirstLetterToUpper();
            }



            var pickUp = new PickUpPoint
            {
                Name = regPickUpPointVM.Name,
                Title = regPickUpPointVM.Title,
                BusStop = regPickUpPointVM.BusStop,
                Description = regPickUpPointVM.Description,
                ObjectState = (int)ContentStatus.Authorized,
                StateLevel = (int)ObjectState.Available,
                DateCreated = DateTime.Now.ToString("yyyy/MM/dd"),
            };

            var model = new Location
            {
                LocationId = regPickUpPointVM.Location.Id,
                Title = regPickUpPointVM.Location.Name,
                Description = regPickUpPointVM.Description,
                LandMark = regPickUpPointVM.Location.Landmark,
                City = regPickUpPointVM.Location.City,
                Area = regPickUpPointVM.Location.Area,
                LCDALabel = ((LCDAToString)regPickUpPointVM?.Location?.LocalCoucilArea).ToUtilString()
            };

            //var model = _mapper.Map<RegLocationObj>(locationVM);

    

            if (!model.ObjValid(out var msg))
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                       $" {msg} "));
            }

            //var locationResult = (await _locationService.GetAllLocation());
            //var isTitleExist = (locationResult).Any(x => x.Title == model?.Title);

            //if (isTitleExist)
            //{
            //    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
            //          $"Location Title is already in use "));
            //}

            var resultData = await _locationService.InsertLocation(model);

            //if (resultData.LocationId < 1)
            //{
            //    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
            //          $" {resultData.ErrorMessage.ErrorMessage} "));
            //}

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

            var list = (await _locationService.GetAllLocation()).Where(x => x.ObjectStatusId == (int)ContentStatus.Authorized).ToList();

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




}

