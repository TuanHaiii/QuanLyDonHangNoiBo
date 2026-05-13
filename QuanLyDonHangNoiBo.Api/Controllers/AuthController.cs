using Microsoft.AspNetCore.Mvc;
using QuanLyDonHangNoiBo.Application.Dtos;
using QuanLyDonHangNoiBo.Application.Services;

namespace QuanLyDonHangNoiBo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly OmsApplicationService _service;

    public AuthController(OmsApplicationService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        return Execute(() => _service.Login(request));
    }

    private ActionResult<T> Execute<T>(Func<T> action)
    {
        try
        {
            return Ok(action());
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
