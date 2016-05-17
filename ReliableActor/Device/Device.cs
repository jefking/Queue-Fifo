using Actors.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Threading.Tasks;

namespace Device
{
    [StatePersistence(StatePersistence.Volatile)]
    internal class Device : Actor, IDevice
    {
        public async Task<bool> Process(Guid id)
        {
            var actorId = ActorId.CreateRandom();

            var geoActor = ActorProxy.Create<IGeoCode>(actorId, new Uri("fabric:/Devices/GeoCodeActorService"));

            var geo = await geoActor.GetPosition();

            var alertActor = ActorProxy.Create<IAlert>(actorId, new Uri("fabric:/Devices/AlertActorService"));

            var alert = await alertActor.Notify(id);
            if (alert)
            {
                var notifActor = ActorProxy.Create<INotification>(actorId, new Uri("fabric:/Devices/NotificationActorService"));

                var notified = await notifActor.Notify();
                if (notified)
                {
                    Console.WriteLine("Actor Notified.");
                }
            }

            return true;
        }
    }
}