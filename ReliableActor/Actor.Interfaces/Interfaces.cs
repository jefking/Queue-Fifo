using Microsoft.ServiceFabric.Actors;
using System;
using System.Threading.Tasks;

namespace GeoCode.Interfaces
{
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
        /// <summary>
        /// TODO: Replace with your own actor method.
        /// </summary>
        /// <returns></returns>
        Task<bool> Notify(Guid g);
    }
}
