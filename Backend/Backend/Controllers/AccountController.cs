using System.Net;
using System.Security.Claims;
using AutoMapper;
using BRMSAPI.Domain;
using BRMSAPI.Utility;
using Configuration;
using Core;
using Domain;
using Extention;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using Service.Contacts;


namespace BRMS.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<Passengers> _signInManager;
    public readonly UserManager<Passengers> _userManager;
    public readonly ITokenService _tokenService;
    public readonly IMapper _mapper;
    public readonly IWebHelper _webHelper;
    private readonly IConfiguration _configuration;


    public AccountController(SignInManager<Passengers> signInManager, UserManager<Passengers> userManager, IMapper mapper, IWebHelper webHelper, ITokenService tokenService, IConfiguration configuration)
    {

        _signInManager = signInManager;
        _userManager = userManager;
        _mapper = mapper;
        _webHelper = webHelper;
        _tokenService = tokenService;
        _configuration = configuration;

    }


    /// <summary>
    /// Passengers Registration 
    /// </summary>
    /// <param name="passengers"></param>
    /// <returns>Sucess created TodoItem</returns>
    /// <response code="200">Returns Success</response>
    /// <response code="400">If the object is null</response>

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegistrationVM userRegistrationVM)
    {
        if (userRegistrationVM == null)
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"this object: {nameof(userRegistrationVM)} is empty"));
        }
        if (!userRegistrationVM.ObjValid(out var msg))
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));
        }
        try

        {
            var emailCheck = await _userManager.FindByEmailAsync(userRegistrationVM.EmailAddress.Trim
                ());
            if (emailCheck != null)
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                       $" {userRegistrationVM.EmailAddress} is Taken "));
            var usernameCheck = await _userManager.FindByNameAsync(userRegistrationVM.UserName);
            if (usernameCheck != null)
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                      $" {userRegistrationVM.UserName} is Taken "));

            var userObj = _mapper.Map<Passengers>(userRegistrationVM);
            userObj.LastLoginIpAddress = _webHelper.GetCurrentIpAddress();
            userObj.DatedJoined = DateTime.UtcNow.ToString("MM/dd/yyyy");
            userObj.EmailToRevalidate = userRegistrationVM.EmailAddress;

           
            var result = await _userManager.CreateAsync(userObj, userRegistrationVM.Password);

            if (!result.Succeeded)
            {
                foreach (var Errmsg in result.Errors)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                        $" {Errmsg.Description} "));
                }
            }

            var tokens = await _userManager.GenerateEmailConfirmationTokenAsync(userObj);

            var subject = $"Confirm Registartion";
            var emailbody = $"Please confirm email address <a href=\"#URL\">Click here </a>";


            //var pageUrl = _webHelper.GetThisPageUrl(true);

            var callbabkUrl = Request.Scheme + "://" + Request.Host + Url.Action("confirm-email", RouteHelper.ACCOUNT, new { token = tokens, email = userObj.Email});

            var body = emailbody.Replace("#URL", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callbabkUrl));

            var retVal = await SendMail(body, userObj.Email, subject);

       
            var roles = new[] { AppConstant.AppUsers };

          
            await _userManager.AddToRolesAsync(userObj, roles);

            return Ok(result);
        }
        catch (Exception ex)
        {
            var message = ex.InnerException;
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }

    }

    [HttpPost("edit")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult> Edit(UserVM userEdit)
    {

        if (userEdit == null)
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"this object: {nameof(userEdit)} is empty"));
        }
        if (!userEdit.ObjValid(out var msg))
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));
        }
        try
        {
            var user = User.FindFirst(ClaimTypes.Email);
            var retUser = await _userManager.FindByEmailAsync(user!.Value);

            var userObj = _mapper.Map<Passengers>(userEdit);

            userObj.LastLoginIpAddress = _webHelper.GetCurrentIpAddress();
            userObj.UpdatedDated = DateTime.UtcNow.ToString("MM/dd/yyyy");

            var result = await _userManager.UpdateAsync(userObj);
            if (!result.Succeeded)
            {
                foreach (var Errmsg in result.Errors)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                        $" {Errmsg.Description} "));
                }

            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }

    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponseVM>> Login(UserLoginVM userLoginVM)
    {
        try
        {
            if (userLoginVM == null)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $" {nameof(userLoginVM)} is empty"));
            }

            var userCheck = (await _userManager.FindByEmailAsync(userLoginVM.EmailAddress.Trim()) ?? await _userManager.FindByNameAsync(userLoginVM.EmailAddress.Trim()));

            if (userCheck == null) return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized, "User does not exist"));

            var signUserIn = await _signInManager.CheckPasswordSignInAsync(userCheck, userLoginVM.Password, false);

            if (!signUserIn.Succeeded) return Unauthorized(new ApiStatusResponse(HttpStatusCode.Unauthorized, "Email or Password does not match our record"));

            var roles = new[] {AppConstant.AppUsers};

            return new UserResponseVM
            {
                EmailAddress = userCheck!.Email,
                UserName = userCheck!.UserName,
                Token = _tokenService.GenerateToken(userCheck, roles),
            };

        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpGet("info")]
    [Authorize]
    public ActionResult<string> Info()
    {
        try
        {
            var test = _webHelper.GetThisPageUrl(true);

            return $"This is the variable test here {test}  Page is protected and only for Authenticated users";
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.InternalServerError));
        }
    }

    [HttpGet("currentUser")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<UserVM>> CurrentUser()
    {
        try
        {
            var user = User.FindFirst(ClaimTypes.Email);
            var retUser = await _userManager.FindByEmailAsync(user!.Value);
            var rolesInfo = await _userManager.GetRolesAsync(retUser);
            var userVm = _mapper.Map<UserVM>(retUser);
            return userVm;
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpPatch("update")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult> Update(EditUserVM model)
    {
        try
        {
            var user = User.FindFirst(ClaimTypes.Email);

            if (user == null)
                return new UnauthorizedResult();

            var currentUser = await _userManager.FindByEmailAsync(user!.Value);

            
            if (!model.ObjValid(out var msg))
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));

      
            currentUser!.FirstName = model.GivenName;
            currentUser.SurnName = model.Surname;
            currentUser.OtherName = model.Surname;
            currentUser.Address = model.HomeAddress;
            currentUser.BirthDate = model.DateOfBirth.ToString("yyyy/MM/dd");

            var retVal = await _userManager.UpdateAsync(currentUser);

            if (!retVal.Succeeded)
            {
                foreach (var error in retVal.Errors)
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, error.Description));
            }

            return Ok(new ApiStatusResponse(HttpStatusCode.OK, "Information Updated Successfully"));
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpPost("change-password")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult> ChangePassword(ChangePasswordVM model)
    {
        try
        {
            var user = User.FindFirst(ClaimTypes.Email);

            if (user == null)
                return new UnauthorizedResult();
            var currentUser = await _userManager.FindByEmailAsync(user!.Value);


            if (!model.ObjValid(out var msg))
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));

            if (model.NewPassword != model.ConfirmNewPassword)
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, "Password does not match"));

            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser!, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, error.Description));
            }

            return Ok(new ApiStatusResponse(HttpStatusCode.BadRequest, "Password Updated Successfully"));
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpGet("emailexist")]
    public async Task<ActionResult> CheckEmailExist([FromQuery] string emailaddress)
    {
        if (string.IsNullOrEmpty(emailaddress))
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $" {emailaddress} is empty or null "));
        }
        try
        {
            if (CommonHelper.IsValidEmail(emailaddress))
            {
                var retVal = await _userManager.FindByEmailAsync(emailaddress) != null;
                if (!retVal)
                {
                    return Ok(new ApiStatusResponse(HttpStatusCode.OK, $"{emailaddress} is available"));
                }
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"{emailaddress} is already in Use "));
            }
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadGateway, $"{emailaddress} is not a valid email address"));
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace!, ex.Source!, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpGet("username-exist")]
    public async Task<ActionResult> CheckUserNameExist([FromQuery] string userName)
    {
        try
        {
            if (string.IsNullOrEmpty(userName) || userName.Length < 3)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $" {userName} must be greater than 3 characters "));
            }
            var retVal = await _userManager.FindByNameAsync(userName) != null;
            if (!retVal)
            {
                return Ok(new ApiStatusResponse(HttpStatusCode.OK, $"{userName} is available"));
            }

            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"{userName} is already in Taken "));
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace!, ex.Source!, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpGet("confirm-email")]
    public async Task<ActionResult> ConfirmEmail([FromQuery] UserConfirmEmail userConfirmEmail)
    {
        if (userConfirmEmail == null)
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"this object: {nameof(userConfirmEmail)} is empty"));
        }

        if (!userConfirmEmail.ObjValid(out var msg))
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));
        }
        try
        {
            var userResult = await _userManager.FindByEmailAsync(userConfirmEmail.Email);
            if (userResult == null)
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                       $" Bad Request"));

            var confirmResult = await _userManager.ConfirmEmailAsync(userResult, userConfirmEmail.Token);
            if (!confirmResult.Succeeded)
            {

                foreach (var Errmsg in confirmResult.Errors)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                        $" {Errmsg.Description} "));
                }
            }

            return Ok(new ApiStatusResponse(HttpStatusCode.OK, "Email is Confirmed"));

        }
        catch (Exception ex)
        {
            var message = ex.InnerException;
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }

    }

    [HttpPost("change-email-request")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult> ChangeEmail([FromQuery] UserChangeEmail userChangeEmail)
    {
        if (userChangeEmail == null)
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"this object: {nameof(userChangeEmail)} is empty"));
        }
        if (!userChangeEmail.ObjValid(out var msg))
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));
        }
        try
        {
            var user = User.FindFirst(ClaimTypes.Email);
            var retUser = await _userManager.FindByEmailAsync(user!.Value);
            if (retUser == null || retUser.FirstName.Length < 2)
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"could not find the records"));
            var token = await _userManager.GenerateChangeEmailTokenAsync(retUser, userChangeEmail.Email);
            if (token == null || token.Length < 2)
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"could not generate token"));
            var pageUrl = _webHelper.GetThisPageUrl(true);
            var newUrl = pageUrl.Replace("change-email-request", "");

            //var emailResult = await _workflowMessageService.SendCustomerEmailValidationMessage(retUser, newUrl, userChangeEmail.Email);
            //if (emailResult < 1)
            //{
            //    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"could not send email, Please try again"));
            //}
            return Ok(new ApiStatusResponse(HttpStatusCode.OK, $"Email Sent, Please Verify Your Email Address"));
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }
    }

    [HttpGet("confirm-changed-email")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult> ConfirmChangedEmail([FromQuery] UserConfirmEmail userConfirmEmail)
    {
        if (userConfirmEmail == null)
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"this object: {nameof(userConfirmEmail)} is empty"));
        }

        if (!userConfirmEmail.ObjValid(out var msg))
        {
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, msg));
        }
        try
        {
            var user = User.FindFirst(ClaimTypes.Email);
            var retUser = await _userManager.FindByEmailAsync(user!.Value);
            if (retUser == null || retUser.FirstName.Length < 2)
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"could not find the records"));

            // Verify the email change request
            var result = await _userManager.ChangeEmailAsync(retUser, userConfirmEmail.Email, userConfirmEmail.Token);
            if (!result.Succeeded)
            {
                foreach (var Errmsg in result.Errors)
                {
                    return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                        $" {Errmsg.Description} "));
                }

              return BadRequest("Email change confirmation failed.");
            }

            return Ok(new ApiStatusResponse(HttpStatusCode.OK, "Email has been updated"));

        }
        catch (Exception ex)
        {
            var message = ex.InnerException;
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
        }

    }


    private async Task<bool> SendMail(string body, string email, string subject)
    {
            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable(_configuration["SMTPTOKEN:MJ_APIKEY_PUBLIC"]), Environment.GetEnvironmentVariable(_configuration["SMTPTOKEN:MJ_APIKEY_PRIVATE"]));
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, "2height@gmail.com")
               .Property(Send.FromName, "BRMS Platform")
               .Property(Send.Subject, $"{subject}")
               .Property(Send.TextPart, "Dear Passenger, welcome to BRMS System! Plese click the link to confirm you email address!")
               .Property(Send.HtmlPart, body)
               .Property(Send.Recipients, new JArray {
                new JObject {
                 {
                        "Email", email}
                 }
                   });
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
             return response.IsSuccessStatusCode;
        }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
             return false;
            }
        }

    }




