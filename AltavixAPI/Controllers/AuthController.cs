using Altavix.Application.Enums;
using Altavix.Application.Features.Auth.Commands.Login;
using Altavix.Application.Features.Auth.Commands.Register;
using Altavix.Application.Features.Auth.Commands.Refresh;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

public class AuthController : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpPost("register-admin")]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterAdminCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    private void SetTokensInsideCookie(string accessToken, string refreshToken)
    {
        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps, 
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"
        };
        Response.Cookies.Append("refreshToken", refreshToken, refreshOptions);

        var accessOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(60),
            Path = "/"
        };
        Response.Cookies.Append("accessToken", accessToken, accessOptions);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            if (result.MessageType == "success" && result.Data != null)
            {
                SetTokensInsideCookie(result.Data.Token, result.Data.RefreshToken);
            }
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshCommand? command)
    {
        try
        {
            command ??= new RefreshCommand();
            
            // If tokens are not in body, try to get from cookies
            if (string.IsNullOrEmpty(command.RefreshToken))
                command.RefreshToken = Request.Cookies["refreshToken"] ?? string.Empty;
                
            if (string.IsNullOrEmpty(command.AccessToken))
                command.AccessToken = Request.Cookies["accessToken"] ?? string.Empty;

            var result = await Mediator.Send(command);
            if (result.MessageType == "success" && result.Data != null)
            {
                SetTokensInsideCookie(result.Data.Token, result.Data.RefreshToken);
            }
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("accessToken", new CookieOptions { HttpOnly = true, Secure = Request.IsHttps, SameSite = SameSiteMode.Lax, Path = "/" });
        Response.Cookies.Delete("refreshToken", new CookieOptions { HttpOnly = true, Secure = Request.IsHttps, SameSite = SameSiteMode.Lax, Path = "/" });
        
        return Ok(new Altavix.Application.Models.ApiResponseDto<string>
        {
            Type = ResponseMessageType.Success,
            Message = "Successfully logged out"
        });
    }
}
