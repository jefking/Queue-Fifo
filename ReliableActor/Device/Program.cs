namespace Device
{
    using Microsoft.ServiceFabric.Actors.Runtime;
    using System;
    using System.Threading;

    internal static class Program
    {
        private static void Main()
        {
            try
            {
                ActorRuntime.RegisterActorAsync<Device>(
                   (context, actorType) => new ActorService(context, actorType, () => new Device())).GetAwaiter().GetResult();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}