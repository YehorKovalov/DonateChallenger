using Identity.API.Models.DTOs;
using Identity.API.Models.Responses;

namespace Identity.API.Services.Abstractions;

public interface IStreamerService
{
    Task<SearchStreamersNicknamesResponse<string>> FindStreamerByNicknameAsync(string nickname);
    Task<GetMinDonatePriceResponse<double?>> GetMinDonatePriceAsync(string streamerId);
    Task<GetStreamerProfileResponse<StreamerProfileDto>> GetStreamerProfileAsync(string streamerId);
    Task<ChangeStreamerProfileDataResponse<double>> ChangeMinDonatePriceAsync(string streamerId, double changeOn);
    Task<ChangeStreamerProfileDataResponse<string>> ChangeStreamerNicknameAsync(string streamerId, string newNickname);
}