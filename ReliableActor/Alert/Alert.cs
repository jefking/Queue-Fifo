namespace Alert
{
    using Actors.Interfaces;
    using Microsoft.ServiceFabric.Actors.Runtime;
    using System;
    using System.Threading.Tasks;

    [StatePersistence(StatePersistence.Persisted)]
    internal class Alert : Actor, IAlert
    {
        public Task<bool> Notify(Guid g)
        {
            return Task.FromResult(true);
        }
    }
}