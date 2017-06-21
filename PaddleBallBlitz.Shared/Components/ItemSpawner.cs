using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace PaddleBallBlitz.Components
{
    class ItemSpawner : Component
    {
        public float Cooldown = -1;
        public float MinInterval = 2;
        public float MaxInterval = 60;
        public int MinCount = 1;
        public int MaxCount = 1;
        public ItemType ItemType;
        public int NumSpawned = 0;
        public int NumAlive = 0;

        public ItemSpawner(ItemType itemType)
        {
            ItemType = itemType;
        }
    }
}
