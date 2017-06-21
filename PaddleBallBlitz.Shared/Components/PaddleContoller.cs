using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace PaddleBallBlitz.Components
{
    class PaddleContoller : Component, IUpdatable
    {
        private Mover _mover;
        private BoxCollider _collider;
        private Vector2 _vel;

        public float Speed { get; set; }

        public PaddleContoller()
        {
            Speed = 450;
            _vel = Vector2.Zero;
        }

        public override void onAddedToEntity()
        {
            _mover = this.getComponent<Mover>();
            _collider = this.getComponent<BoxCollider>();
        }

        public void update()
        {
            if (Input.isKeyDown(Keys.Down))
                _vel.Y = Speed;
            else if (Input.isKeyDown(Keys.Up))
                _vel.Y = -Speed;
            else
                _vel.Y = 0f;

            var deltaMovement = _vel * Time.deltaTime;
            entity.position += deltaMovement;

            if (entity.position.Y - _collider.height / 2 <= 0 || entity.position.Y + _collider.height / 2 >= Screen.height)
            {
                entity.position -= deltaMovement;
                _vel.Y = 0;
            }
        }
    }
}
