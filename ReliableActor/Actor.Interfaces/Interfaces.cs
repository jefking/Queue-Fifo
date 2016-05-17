namespace Actors.Interfaces
{
    using Microsoft.ServiceFabric.Actors;
    using System;
    using System.Threading.Tasks;

    public interface INotification : IActor
    {
        Task<bool> Notify();
    }

    public interface IGeoCode : IActor
    {
        Task<double[]> GetPosition();
    }

    public interface IDevice : IActor
    {
        Task<bool> Process(Guid id);
    }

    public interface IAlert : IActor
    {
        Task<bool> Notify(Guid g);
    }
}
