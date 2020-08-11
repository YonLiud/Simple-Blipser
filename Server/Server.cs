using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Server : BaseScript
    {
        public Server()
        {
            EventHandlers["serverBlip"] += new Action<Vector3, string>(OnBlipRequest);
        }
        public void OnBlipRequest(Vector3 position, string name)
        {
            TriggerClientEvent("clientBlip", position, name);
        }
    }
}
