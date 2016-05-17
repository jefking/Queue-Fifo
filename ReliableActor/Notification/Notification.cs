namespace Notification
{
    using Actors.Interfaces;
    using Microsoft.ServiceFabric.Actors.Runtime;
    using System.Threading.Tasks;

    [StatePersistence(StatePersistence.Persisted)]
    internal class Notification : Actor, INotification
    {
        public Task<bool> Notify()
        {
            return Task.FromResult(true);
        }
    }
}
