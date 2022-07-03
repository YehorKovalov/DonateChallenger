using Identity.API.Models.Responses;

namespace Identity.API.Services.Abstractions;

public interface IStreamerService
{
    Task<SearchStreamersNicknamesResponse<string>> FindStreamerByNicknameAsync(string nickname);
    Task<GetMinDonatePriceResponse<double?>> GetMinDonatePriceAsync(string streamerId);
    Task<ChangeMinDonatePriceResponse> ChangeMinDonatePriceAsync(string streamerId, double changeOn);
}