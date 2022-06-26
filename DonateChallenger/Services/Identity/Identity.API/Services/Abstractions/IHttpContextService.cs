namespace Identity.API.Services.Abstractions;

public interface IHttpContextService
{
    HttpContext GetHttpContext();
}