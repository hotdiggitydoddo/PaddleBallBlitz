using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PaddleBallBlitz.Components
{
    class BallLogic : Component, IUpdatable
    {
        private Mover _mover;
        private CircleCollider _collider;

        private SoundEffect _paddleHit;
        private SoundEffect _ceilingHit;

        public Vector2 Velocity;
        public float MaxSpeed { get; set; }

        public Vector2 Direction
        {
            get
            {
                var tmp = new Vector2(Velocity.X, Velocity.Y);
                tmp.Normalize();
                return tmp;
            }
        }
        public BallLogic(SoundEffect paddle, SoundEffect ceiling)
        {
            Velocity = new Vector2(-495, -95);
            MaxSpeed = 590;

            _paddleHit = paddle;
            _ceilingHit = ceiling;

        }

        public void Reset()
        {
            Velocity = Vector2.Zero;
        }

        public void Start(bool? playerScoredLast = null)
        {
            //random speed
            Velocity = new Vector2(Nez.Random.range(300f, 500f), Nez.Random.range(45f, 95f));

            //if fresh game, completely randomize the direction;
            if (!playerScoredLast.HasValue)
            {
                if (Nez.Random.chance(50))
                {
                    Velocity.X *= -1;
                }
                if (Nez.Random.chance(50))
                {
                    Velocity.Y *= -1;
                }
            }
            //start the ball going in the direction of who lost the last round
            else if (!playerScoredLast.Value)
            {
                Velocity.X *= -1;
                if (Nez.Random.chance(50))
                {
                    Velocity.Y *= -1;
                }
            }
        }
        public override void onAddedToEntity()
        {
            _mover = this.getComponent<Mover>();
            _collider = this.getComponent<CircleCollider>();
        }
        public void update()
        {
            var deltaMovement = Velocity * Time.deltaTime;

            CollisionResult c;

            if (entity.getComponent<CircleCollider>().collidesWithAny(ref deltaMovement, out c))
            {
                Velocity = Vector2.Reflect(Velocity, c.normal);

                var coef = (entity.position.Y - c.collider.bounds.y) / (c.collider.bounds.height / 2) - 1; // this will give you a number between -1 (top) and 1 (bottom)
                Velocity.Y = MaxSpeed * coef;
                _paddleHit.Play();
            }

            entity.position += deltaMovement;

            if (entity.position.X - _collider.radius <= 0 || entity.position.X + _collider.radius >= Screen.width)
            {
                entity.position -= deltaMovement;
                Velocity.X *= -1;
                _ceilingHit.Play();
            }
            if (entity.position.Y - _collider.radius <= 0 || entity.position.Y + _collider.radius >= Screen.height)
            {
                entity.position -= deltaMovement;
                Velocity.Y *= -1;
                _ceilingHit.Play();
            }

            if (Velocity.X == 0)
            {
                
            }

        }
    }
}
