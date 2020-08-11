using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class Client : BaseScript
    {
        const int BLIP_TIMER = 10; //In Seconds
        bool onCooldown = false;
        public Client()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["clientBlip"] += new Action<Vector3, string>(OnRequestBlip);
        }
        private void OnClientResourceStart(string resourceName)
        {
            RegisterCommand("blipme", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (onCooldown)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 128, 0 },
                        multiline = true,
                        args = new[] { "Hold On", $"There's Already an Active Blip you set. ^* ^3Please Wait!" }
                    });
                    return;
                }
                Vector3 position = Game.PlayerPed.Position;
                string name = Game.Player.Name;
                TriggerServerEvent("serverBlip", position, name);
                Cooldown();
            }), false);
        }
        private void OnRequestBlip(Vector3 position, string name)
        {
            Blip blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.Friend;
            blip.Name = $"{name}'s Shared Location";
            blip.Color = BlipColor.Yellow;
            blip.IsFriendly = true;
            RemoveBlip(blip);
        }
        private async void RemoveBlip(Blip blip)
        {
            await Delay(BLIP_TIMER * 1000);
            blip.Delete();
        }

        private async void Cooldown()
        {
            onCooldown = true;
            Debug.WriteLine("Cooldown Set");
            await Delay(BLIP_TIMER * 1000);
            onCooldown = false;
            Debug.WriteLine("Your Cooldown has expired");

        }

    }
}
