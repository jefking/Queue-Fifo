namespace GeoCode
{
    using Actors.Interfaces;
    using Microsoft.ServiceFabric.Actors.Runtime;
    using System;
    using System.Threading.Tasks;

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
