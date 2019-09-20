using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Chat_Commands
{
    public class Main : BaseScript
    {
        public Main()
        {
            RegisterCommand("ping", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] {"[Ping]", "PONG!"}
                });
            }), false);

            TriggerEvent("chat:addSuggestion", "/ping", "help text", new[]
            {
                new { name="paramName1", help="param description 1" },
                new { name="paramName2", help="param description 2" }
            });
        }
    }
}
