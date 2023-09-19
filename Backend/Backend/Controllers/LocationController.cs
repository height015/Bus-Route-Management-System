using System;
using System.Collections.Generic;
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

namespace BRMSAPI.Controllers;

public class LocationController : BaseApiController
{
	private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService, UserManager<Passengers> userManager, IMapper mapper, IHttpContextAccessor accessor) : base(userManager, mapper, accessor)
    {
        _locationService = locationService;

    }






    /// <summary>
    /// add Location 
    /// </summary>
    /// <param name="LocationVM"></param>
    /// <returns> IsSuccessful = true, Id = id,  Error = "" </returns>
    /// <response code="200">Returns Success</response>
    /// <response code="400">If the object is null</response>
    /// <response code="401">If user is Unauthorized </response>

    [ProducesResponseType(typeof(ResponseResObj), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("add-location")]
    public async Task<ActionResult> AddLocation(RegLocationVM locationVM)
    {
        try
        {
            var userData = await CurrentUser();

            if (string.IsNullOrEmpty(userData.UserId))
            {

                return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized,
                      $" Your session has expired "));
            }

            var model = new Location
            {
                Title = locationVM.Name,
                Description = locationVM.Description,
                LandMark = locationVM.LandMark,
                City = locationVM.City,
                Area = locationVM.Area,
                LCDALabel = ((LCDAToString)locationVM.LocalCoucilArea).ToUtilString()
            };

            //var model = _mapper.Map<RegLocationObj>(locationVM);

            model.ObjectStatusId = (int)ContentStatus.Authorized;

            


            if (!model.ObjValid(out var msg))
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                       $" {msg} "));
            }

            var locationResult = (await _locationService.GetAllLocation());
            var isTitleExist = (locationResult).Any(x => x.Title == model.Title);

            if (isTitleExist)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                      $"Location Title is already in use "));
            }

            var resultData = await _locationService.InsertLocation(model);

            if (resultData.LocationId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                      $" {resultData.ErrorMessage.ErrorMessage} "));
            }

            return Ok(new { IsSuccessful = true, Id = resultData.LocationId,  Error = "" });

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

            model = list.Select(x=> new LocationMod
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
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,"Invalid Identity"));
            }


            var reVal = (await _locationService.GetLocationById(Id));


            if (reVal == null || reVal.LocationId < 1)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,"Invalid or Empty Object"));
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
                CityLabel= reVal.CityLabel,
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



