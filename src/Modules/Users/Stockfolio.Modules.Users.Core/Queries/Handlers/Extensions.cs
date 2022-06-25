﻿using Stockfolio.Modules.Users.Core.DTO;
using Stockfolio.Modules.Users.Core.Entities;

namespace Stockfolio.Modules.Users.Core.Queries.Handlers;

internal static class Extensions
{
    private static readonly Dictionary<UserState, string> States = new()
    {
        [UserState.Active] = UserState.Active.ToString().ToLowerInvariant(),
        [UserState.Locked] = UserState.Locked.ToString().ToLowerInvariant()
    };

    public static UserDto AsDto(this User member)
        => member.Map<UserDto>();

    public static UserDetailsDto AsDetailsDto(this User user)
    {
        return user.Map<UserDetailsDto>();
    }

    private static T Map<T>(this User user) where T : UserDto, new()
        => new()
        {
            UserId = user.Id,
            Email = user.Email,
            State = States[user.State],
            Roles = user.UserRoles.Select(x => x.Role.Name),
            CreatedAt = user.CreatedAt
        };
}