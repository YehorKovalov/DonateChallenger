using Identity.API.Models.Requests;
using Identity.API.Models.Responses;

namespace Identity.API.Services.Abstractions;

public interface IAccountManagerService
{
    Task<DeleteUserByIdResponse<bool>> DeleteUserByIdAsync(string userId);
    Task<ManagerUpdateProfileResponse> UpdateUserProfileAsync(ManagerUpdateUserProfileRequest request);
    Task<ManagerUpdateProfileResponse> UpdateStreamerProfileAsync(ManagerUpdateStreamerProfileRequest request);
}