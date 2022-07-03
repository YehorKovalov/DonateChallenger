using Identity.API.Models.Responses;

namespace Identity.API.Services.Abstractions;

public interface IStreamerService
{
    Task<SearchStreamersNicknamesResponse<string>> FindStreamerByNickname(string nickname);
}