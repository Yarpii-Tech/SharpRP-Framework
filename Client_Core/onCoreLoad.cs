using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client_Core
{
    public class onCoreLoad : BaseScript
    {
        public onCoreLoad()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(onClientResourceStart);
        }

        private void onClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            disableDispatchServices(); // Disable AI emergency dispatch services
        }

        private void disableDispatchServices()
        {
            for (int i = 0; i <= 20; i++)
            {
                EnableDispatchService(i, false);
            }
        }
    }
}
