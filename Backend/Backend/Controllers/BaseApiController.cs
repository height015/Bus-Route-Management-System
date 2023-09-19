using System;
using Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Models;
using AutoMapper;
using BRMSAPI.Domain;
using Service.Contacts;
using Core;
using Backend.Utility;

namespace BRMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController  : ControllerBase
{

    public readonly UserManager<Passengers> _userManager;
    public readonly IMapper _mapper;
    private readonly IHttpContextAccessor _accessor;


    public BaseApiController(UserManager<Passengers> userManager, IMapper mapper, IHttpContextAccessor accessor)
    {
        _userManager = userManager;
        _mapper = mapper;
        _accessor = accessor;
    }


 
    [Authorize(Policy = "UserPolicy")]
    protected async Task<UserVM> CurrentUser()
    {
        try
        {
            var user = User?.FindFirst(ClaimTypes.Email);
            if (user != null)
            {
                var retUser = await _userManager?.FindByEmailAsync(user!.Value);
                var rolesInfo = await _userManager?.GetRolesAsync(retUser);
                var userVm = _mapper.Map<UserVM>(retUser);
                return userVm;
            }
            return new UserVM();
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return new UserVM();
        }
    }



}
