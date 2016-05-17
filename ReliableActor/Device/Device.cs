using Alert.Interfaces;
using Device.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Threading.Tasks;

namespace Device
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Volatile)]
    internal class Device : Actor, IDevice
    {
        public async Task<bool> Process(Guid id)
        {
            // Create a randomly distributed actor ID
            var actorId = ActorId.CreateRandom();

            // This only creates a proxy object, it does not activate an actor or invoke any methods yet.
            var geoActor = ActorProxy.Create<IGeoCode>(actorId, new Uri("fabric:/MyApp/MyActorService"));
            var alertActor = ActorProxy.Create<IAlert>(actorId, new Uri("fabric:/MyApp/MyActorService"));

            // This will invoke a method on the actor. If an actor with the given ID does not exist, it will be activated by this method call.
            await myActor.DoWorkAsync();

            return true;
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see http://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("count", 0);
        }
    }
}