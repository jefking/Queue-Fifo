using Actors.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DataEntry.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values 
        public async Task<bool> Get(Guid id)
        {
            var actorId = ActorId.CreateRandom();
            var device = ActorProxy.Create<IDevice>(actorId, new Uri("fabric:/Devices/DeviceActorService"));
            return await device.Process(id);
        }
    }
}
