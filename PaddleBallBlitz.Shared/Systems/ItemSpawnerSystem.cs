using System;
using Microsoft.Xna.Framework;
using Nez;
using PaddleBallBlitz.Components;

namespace PaddleBallBlitz.Systems
{
    class ItemSpawnerSystem : EntityProcessingSystem
    {

        public ItemSpawnerSystem(Matcher matcher) : base(matcher)
        {
        }

        public override void process(Entity entity)
        {
            var spawner = entity.getComponent<ItemSpawner>();
            if (spawner.NumAlive <= 0)
                spawner.enabled = true;

            if (!spawner.enabled)
                return;

            if (spawner.Cooldown <= -1)
            {
                scheduleSpawn(spawner);
                spawner.Cooldown /= 4;
            }

            spawner.Cooldown -= Time.deltaTime;
            if (spawner.Cooldown <= 0)
            {
                scheduleSpawn(spawner);

                for (var i = 0; i < Nez.Random.range(spawner.MinCount, spawner.MaxCount); i++)
                {
                    Entity e = null;

                    switch (spawner.ItemType)
                    {
                        case ItemType.FireballPowerUp:
                            e = EntityFactory.Instance.CreateEntity(EntityType.FireballPowerUp);
                            break;
                    }
                    e.position = GetSpawnPoint();
                    spawner.NumSpawned++;
                    spawner.NumAlive++;
                }

                if (spawner.NumAlive > 0)
                    spawner.enabled = false;
            }
        }

        private void scheduleSpawn(ItemSpawner spawner)
        {
            spawner.Cooldown = Nez.Random.range(spawner.MinInterval, spawner.MaxInterval);
        }

        private Vector2 GetSpawnPoint()
        {
            var x = Nez.Random.range(100, Screen.width - 100);
            var y = Nez.Random.range(100, Screen.height - 100);
            return new Vector2(x, y);
        }
    }
}
