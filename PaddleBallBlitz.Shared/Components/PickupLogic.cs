using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using PaddleBallBlitz.Systems;

namespace PaddleBallBlitz.Components
{
    class PickupLogic : Component, IUpdatable
    {
        private Collider _collider;
        private ItemSpawner _spawner;
        private CollisionResult _collisionResult;
        private ItemType _type;

        public override void onAddedToEntity()
        {
            _collider = entity.getComponent<Collider>();
        }

        public void update()
        {
            if (_collider.collidesWithAny(out _collisionResult))
            {
                var ball = _collisionResult.collider.entity;
                ball.addComponent(new Fireball());


                var spawners = entity.scene.findComponentsOfType<ItemSpawner>();

                var thisSpawner = spawners.SingleOrDefault(x => x.ItemType == _type);
                if (thisSpawner != null)
                    thisSpawner.NumAlive--;
                
                entity.destroy();
            }
        }
    }
}
