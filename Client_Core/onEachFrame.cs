using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client_Core
{
    class onEachFrame : BaseScript
    {
        public onEachFrame()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(onEachFrameHandler);
        }

        private void onEachFrameHandler(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Tick += onEachFrameTask;
        }

        private async Task onEachFrameTask()
        {
            while (true)
            {
                await BaseScript.Delay(0);

                if (GetPlayerWantedLevel(PlayerId()) != 0)
                {
                    ClearPlayerWantedLevel(PlayerId());
                    SetPlayerWantedLevel(PlayerId(), 0, false);
                    SetPlayerWantedLevelNow(PlayerId(), false);
                }
            }
        }
    }
}
