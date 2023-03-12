﻿using Microsoft.Extensions.Logging;

namespace MareSynchronos.Services.Mediator;

public abstract class MediatorSubscriberBase : IMediatorSubscriber
{
    protected ILogger Logger { get; }
    public MareMediator Mediator { get; }
    protected MediatorSubscriberBase(ILogger logger, MareMediator mediator)
    {
        Logger = logger;

        Logger.LogTrace("Creating {type} ({this})", GetType().Name, this);
        Mediator = mediator;
    }

    protected void UnsubscribeAll()
    {
        Logger.LogTrace("Unsubscribing from all for {type} ({this})", GetType().Name, this);
        Mediator.UnsubscribeAll(this);
    }
}
