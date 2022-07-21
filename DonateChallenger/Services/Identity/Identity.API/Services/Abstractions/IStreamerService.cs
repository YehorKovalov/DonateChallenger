using Identity.API.Models.DTOs;
using Identity.API.Models.Responses;

namespace Identity.API.Services.Abstractions;

public interface IStreamerService
{
    Task<SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto>> FindStreamerByNicknameAsync(string nickname);
    Task<GetMinDonatePriceResponse<double?>> GetMinDonatePriceAsync(string streamerId);
    Task<GetStreamerProfileResponse<StreamerProfileDto>> GetStreamerProfileAsync(string streamerId);
    Task<ChangeProfileDataResponse<double>> ChangeMinDonatePriceAsync(string streamerId, double changeOn);
}