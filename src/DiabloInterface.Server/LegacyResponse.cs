using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.DiabloInterface.Server.Handlers;

namespace Zutatensuppe.DiabloInterface.Server
{
    class LegacyResponse
    {
        public bool Success { get; set; }
        public List<ItemInfo> Items { get; set; }

        public LegacyResponse(Response response)
        {
            Items = response.Payload == null
                ? new List<ItemInfo>()
                : ((ItemResponsePayload)response.Payload).Items;
            Success = Items.Count > 0;
        }
    }
}
