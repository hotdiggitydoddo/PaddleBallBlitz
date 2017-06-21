using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace PaddleBallBlitz.Components
{
    class CpuLogic : Component, IUpdatable
    {
        private Mover _mover;
        private BoxCollider _collider;
        private Vector2 _vel;

        public float MaxSpeed { get; set; }

        public CpuLogic()
        {
            MaxSpeed = 550; //650 is harder to beat!!
            _vel = Vector2.Zero;
        }

        public override void onAddedToEntity()
        {
            _mover = this.getComponent<Mover>();
            _collider = this.getComponent<BoxCollider>();
        }

        public void update()
        {
            var ball = entity.scene.entities.findEntity("ball");
            var ballVel = ball.getComponent<BallLogic>().Velocity;
            var ballColl = ball.getComponent<CircleCollider>();

            if (ballVel.X > 0 && ball.position.X + ballColl.radius > Screen.center.X)
            {
                // move towards the ball
                // for now, try to get the paddle centered at the ball
                if (ball.position.Y != entity.position.Y)
                {
                    var timeTilCollision = ((Screen.width - 32 -_collider.width) - ball.position.X) / (ballVel.X);
                    var distanceWanted = (entity.position.Y ) - (ball.position.Y );
                    var velocityWanted = -distanceWanted / timeTilCollision;
                    if (velocityWanted > MaxSpeed)
                    {
                        _vel.Y = MaxSpeed;
                    }
                    else if (velocityWanted < -MaxSpeed)
                    {
                        _vel.Y = -MaxSpeed;
                    }
                    else
                    {
                        _vel.Y = velocityWanted;
                    }

                    CollisionResult c;
                    var deltaMovement = _vel * Time.deltaTime;
                    entity.position += deltaMovement;

                    if (entity.position.Y - _collider.height / 2 <= 0 || entity.position.Y + _collider.height / 2 >= Screen.height)
                    {
                        entity.position -= deltaMovement;
                        _vel.Y = 0;
                    }

                }
                else
                {
                    _vel.Y = 0;
                }
            }
            else
            {
                _vel.Y = 0;
            }
        }
    }
}
