﻿using Stockfolio.Shared.Infrastructure.Postgres;

namespace Stockfolio.Modules.Users.Core.DAL;

internal class UsersUnitOfWork : PostgresUnitOfWork<UsersDbContext>
{
    public UsersUnitOfWork(UsersDbContext dbContext) : base(dbContext)
    {
    }
}