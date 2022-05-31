﻿using Stockfolio.Shared.Abstractions.Events;

namespace Stockfolio.Modules.Users.Core.Events;
internal record UserStateUpdated(Guid UserId, string State) : IEvent;