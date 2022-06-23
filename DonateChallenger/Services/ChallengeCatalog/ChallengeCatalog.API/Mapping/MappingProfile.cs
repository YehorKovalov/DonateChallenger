using AutoMapper;
using ChallengeCatalog.API.Data.Entities;
using ChallengeCatalog.API.Models.DTOs;
using ChallengeCatalog.API.Models.Response;

namespace ChallengeCatalog.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ChallengeEntity, ChallengeDto>();

        CreateMap<ChallengeDto, SkippedChallengeDto>();
        CreateMap<ChallengeDto, CompletedChallengeDto>();
        CreateMap<ChallengeDto, CurrentChallengeDto>();

        CreateMap<GetPaginatedChallengesResponse<ChallengeDto>, GetPaginatedChallengesResponse<SkippedChallengeDto>>();
        CreateMap<GetPaginatedChallengesResponse<ChallengeDto>, GetPaginatedChallengesResponse<CompletedChallengeDto>>();
        CreateMap<GetPaginatedChallengesResponse<ChallengeDto>, GetPaginatedChallengesResponse<CurrentChallengeDto>>();
    }
}