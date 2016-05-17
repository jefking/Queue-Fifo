using GeoCode.Interfaces;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Threading.Tasks;

namespace GeoCode
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class GeoCode : Actor, IGeoCode
    {
        public Task<double[]> GetPosition()
        {
            var random = new Random();

            double lat = random.Next();
            double lng = random.Next();

            return Task.FromResult(new double[] { lat, lng });
        }
    }
}
