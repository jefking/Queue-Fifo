﻿using Microsoft.ServiceFabric.Actors;
using System.Threading.Tasks;

namespace Actor1.Interfaces
{
    /// <summary>
    /// This interface represents the actions a client app can perform on an actor.
    /// It MUST derive from IActor and all methods MUST return a Task.
    /// </summary>
    public interface IDeviceActor : IActor
    {
        Task<int> GetCountAsync();

        Task SetCountAsync(int count);
    }
}
