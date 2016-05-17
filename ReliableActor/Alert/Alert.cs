using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Threading.Tasks;

namespace Alert
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class Alert : Actor, IAlert
    {
        public Task<bool> Notify(Guid g)
        {
            return Task.FromResult(true);
        }
    }
}
