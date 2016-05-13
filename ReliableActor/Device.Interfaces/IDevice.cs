using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Device.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IDevice : IActor
    {
        Task<bool> Process(Guid id);
    }
}
