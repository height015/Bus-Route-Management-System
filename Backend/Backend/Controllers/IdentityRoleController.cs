
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JosephProfile.Model;
using Models;
using AutoMapper;
using BRMSAPI.Domain;
using Configuration;

namespace BRMS.Controllers;

//[Authorize(Policy = "AdminPolicy")]
[Route("api/[controller]")]
[ApiController]
public class IdentityRoleController : ControllerBase
{
	private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<Passengers> _userManager;
    public readonly IMapper _mapper;

    public IdentityRoleController(RoleManager<IdentityRole> roleManager, UserManager<Passengers> userManager, IMapper mapper)
	{
		_roleManager = roleManager;
        _userManager = userManager;
        _mapper = mapper;

    }
  
    [HttpPost("create-role")]

    public async Task<ActionResult> CreateRole([MinLength(3), Required] string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length <=2)
			return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $" {name} can not be empty and must be greater than 2 characters"));

		try
		{
            var retVal = await _roleManager.CreateAsync(new IdentityRole(name));
            if (!retVal.Succeeded)
            {
                foreach (var error in retVal.Errors)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, error.Description));
                }
            }
            return Ok(new ApiStatusResponse(HttpStatusCode.Created, $" Role {name} is Succesfully Created"));
        }
		catch (Exception ex)
		{
            ErrorUtilTools.LogErr(ex.StackTrace!, ex.Source!, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }

    }

 
    [HttpGet("list")]
    public virtual async Task<ActionResult> List()
    {
        try
        {
            var retVal = _roleManager.Roles.Select(x => new IdentityUserRoleVM
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            if (retVal.Count < 1 || retVal == null)
                return NoContent();
            return Ok(await Task.FromResult(retVal));
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpGet("details-role")]
    public async Task<ActionResult> Details(string Id)
    {
        if (string.IsNullOrEmpty(Id) || Id.Length < 4 )
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $" {Id} can not be empty and must be greater than 3 characters"));
        try
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role != null )
            {
                List<UserVM> member = new List<UserVM>();
                List<UserVM> nonMember = new List<UserVM>();

                foreach (var user in _userManager!.Users)
                {
                    var list = await _userManager.IsInRoleAsync(user, role.Name) ? member : nonMember;
                    list.Add(_mapper.Map<UserVM>(user));
                }
                return Ok(new RoleEditVM
                {
                    Role = role,
                    Memeber = member,
                    NonMemeber = nonMember
                });
            }
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpPost("update-user-role")]
 
    public async Task<ActionResult> UpdateRoleWithUsers(RoleEditVM roleEditVM)
    {
        if (roleEditVM ==  null)
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $" {roleEditVM} can not be empty or null"));
        try
        {
            IdentityResult result;
            foreach (string userId in roleEditVM.AddIds ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(user, roleEditVM.RoleName);
            }

            foreach (string userId in roleEditVM.DeleteIds ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(user, roleEditVM.RoleName);
            }

            return Ok();

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
     
    }



}

