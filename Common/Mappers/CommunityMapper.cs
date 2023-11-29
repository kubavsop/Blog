using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities.Database;

namespace Blog.API.Common.Mappers;

internal static class CommunityMapper
{
    public static IEnumerable<CommunityDto> CommunitiesToCommunitiesDto(IEnumerable<Community> communities)
    {
        return communities.Select(CommunityToCommunityDto);
    }

    private static CommunityDto CommunityToCommunityDto(Community community)
    {
        return new CommunityDto
        {
            Id = community.Id,
            CreateTime = community.CreateTime,
            Name = community.Name,
            Description = community.Description,
            IsClosed = community.IsClosed,
            subscribersCount = community.SubscribersCount
        };
    }
}