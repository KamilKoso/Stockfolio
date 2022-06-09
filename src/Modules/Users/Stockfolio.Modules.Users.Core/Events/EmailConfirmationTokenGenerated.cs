﻿using Stockfolio.Shared.Abstractions.Events;

namespace Stockfolio.Modules.Users.Core.Events;
internal record EmailConfirmationTokenGenerated(string Username, string Email, string Token) : IEvent;