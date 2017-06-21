using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Particles;

namespace PaddleBallBlitz.Components
{
    class Fireball : Component, IUpdatable
    {
        private ParticleEmitter _particles;
        private float _removalCountDown;

        private BallLogic _ballLogic;
        public Vector2 OriginalVelocity { get; set; }
        public float TimeToLive { get; set; }
        public float VelocityOffset { get; set; }
        public bool IsDead => TimeToLive <= 0;


        public override void onAddedToEntity()
        {
            _ballLogic = entity.getComponent<BallLogic>();
            _particles = entity.addComponent(new ParticleEmitter(EntityFactory.Instance.ParticleConfigs["Fireball"], false));
            _removalCountDown = 2f;
            TimeToLive = Nez.Random.range(5f, 10f);
        }


        public void update()
        {
            if (!IsDead)
            {
                if (OriginalVelocity == Vector2.Zero)
                {
                    OriginalVelocity = _ballLogic.Velocity;
                    VelocityOffset = 95f;
                }

                _ballLogic.Velocity += new Vector2(VelocityOffset * Time.deltaTime) * _ballLogic.Direction;
                TimeToLive -= Time.deltaTime;

                _particles.emit(Nez.Random.nextInt(45));
            }
            else
            {
                _removalCountDown -= Time.deltaTime;
                if (_removalCountDown <= 0)
                {
                    var particles = entity.getComponent<ParticleEmitter>();
                    entity.removeComponent(this);
                    entity.removeComponent(particles);
                }
            }
        }
    }
}
