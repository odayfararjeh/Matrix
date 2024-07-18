using Common.Constants;
using Common.Enums;
using DTO.IdentityDtos;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sajaya_Task.Helper;
using System.Net;

namespace Sajaya_Task.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : APIBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenUtility _tokenService;
        private readonly IPasswordUtility _passwordService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityController(IUserRepository userRepository,
                                  ITokenUtility tokenService,
                                  IPasswordUtility passwordService,
                                  IHttpContextAccessor httpContextAccessor,
                                  ILogger<IdentityController> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpGet("Authenticate")]
        public async Task<ActionResult> Login([FromQuery] LoginDto model)
        {
            try
            {
                UserDto userModel = await _userRepository.GetUserAsync(model.UserName);

                if (userModel == null)
                {
                    var errors = new ModelErrorCollection();
                    errors.Add(new ModelError("User does not exist"));

                    return BadRequest(IdentityHelper.GenerateResponseModel(ErrorEnum.UserNameIsInvalid, errors, null, HttpStatusCode.BadRequest));
                }

                if (!_passwordService.ValidatePassword(model.Password, userModel.Password))
                {
                    var errors = new ModelErrorCollection();
                    errors.Add(new ModelError("Password Is Invalid"));

                    return BadRequest(IdentityHelper.GenerateResponseModel(ErrorEnum.PasswordIsInvalid, errors, null, HttpStatusCode.BadRequest));
                }

                userModel.LastLoginDate = DateTime.Now;

                bool result = await _userRepository.UpdateUserLastLoginDateAsync(userModel);

                if (!result)
                {
                    var errors = new ModelErrorCollection();
                    errors.Add(new ModelError("User Last Login Date Update Failed"));

                    return BadRequest(IdentityHelper.GenerateResponseModel(ErrorEnum.LastLoginDateUpdateFailed, errors, null, HttpStatusCode.BadRequest));
                }


                Task<string> accessTokenTask = _tokenService.GenerateAccessTokenAsync(userModel);

                await Task.WhenAll(accessTokenTask);

                userModel.AccessToken = await accessTokenTask;

                if (string.IsNullOrEmpty(userModel.AccessToken))
                {
                    var errors = new ModelErrorCollection();
                    errors.Add(new ModelError("Update User Access Token Failed"));

                    return BadRequest(IdentityHelper.GenerateResponseModel(ErrorEnum.UpdateUserAccessTokenFailed, errors, null, HttpStatusCode.BadRequest));
                }

                return Ok(IdentityHelper.GenerateResponseModel(null, null, userModel, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                var errors = new ModelErrorCollection();
                errors.Add(new ModelError(ex));

                return StatusCode((int)HttpStatusCode.InternalServerError, IdentityHelper.GenerateResponseModel(ErrorEnum.ExceptionError, errors, null, HttpStatusCode.InternalServerError, ex));
            }
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                bool result = await _tokenService.DeleteUserAccessTokenAsync(CurrentUserId);

                return Ok(IdentityHelper.GenerateResponseModel(null, null, result, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                var errors = new ModelErrorCollection();
                errors.Add(new ModelError(ex));

                return StatusCode((int)HttpStatusCode.InternalServerError, IdentityHelper.GenerateResponseModel(ErrorEnum.ExceptionError, errors, null, HttpStatusCode.InternalServerError, ex));
            }
        }
    }
}
