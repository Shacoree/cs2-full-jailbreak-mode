using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreak.JailItems
{
    public abstract class JailItem
    {
        public readonly int costT;
        public readonly int costCT;
        public CCSPlayerController player { get; set; }

        public string name;
        public JailItem()
        {

        }
        public virtual void GiveItemToPlayer()
        {

        }

    }
}
