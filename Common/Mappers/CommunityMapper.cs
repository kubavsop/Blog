using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Common.Mappers;

internal static class CommunityMapper
{

    public static RoleResponseDto RoleResponseToRoleResponseDto(RoleResponse response)
    {
        return new RoleResponseDto
        {
            Role = response.Role
        };
    }
    public static CommunityFullDto CommunityFullToCommunityFullDto(CommunityFull community)
    {
        return new CommunityFullDto
        {
            Id = community.Id,
            CreateTime = community.CreateTime,
            Name = community.Name,
            Description = community.Description,
            IsClosed = community.IsClosed,
            SubscribersCount = community.SubscribersCount,
            Administrators = UserMapper.UsersToUsersDto(community.Administrators)
        };
    }

    public static Community CreateCommunityDtoToCreateCommunity(CreateCommunityDto communityDto)
    {
        return new Community
        {
            Name = communityDto.Name,
            Description = communityDto.Description,
            IsClosed = communityDto.IsClosed
        };
    }
    public static IEnumerable<CommunityUserDto> CommunitiesUserToCommunitiesUserDto(
        IEnumerable<CommunityUser> communities)
    {
        return communities.Select(CommunityUserToCommunityUserDto);
    }

    public static IEnumerable<CommunityDto> CommunitiesToCommunitiesDto(IEnumerable<Community> communities)
    {
        return communities.Select(CommunityToCommunityDto);
    }

    private static CommunityUserDto CommunityUserToCommunityUserDto(CommunityUser communityUser)
    {
        return new CommunityUserDto
        {
            UserId = communityUser.UserId,
            CommunityId = communityUser.CommunityId,
            Role = communityUser.Role
        };
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
            SubscribersCount = community.SubscribersCount
        };
    }
}