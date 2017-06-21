using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Particles;
using Nez.Sprites;
using Nez.TextureAtlases;
using Nez.Timers;
using PaddleBallBlitz.Components;

namespace PaddleBallBlitz
{
    class EntityFactory
    {
        private static EntityFactory _instance;
        private TextureAtlas _atlas;
        private Scene _scene;

        public Dictionary<string, ParticleEmitterConfig> ParticleConfigs;

        private EntityFactory() { }

        public static EntityFactory Instance => _instance ?? (_instance = new EntityFactory());

        public bool Initialize(Scene scene)
        {
            _scene = scene;
            try
            {
                _atlas = scene.content.Load<TextureAtlas>("SpriteAtlas");
                ParticleConfigs = new Dictionary<string, ParticleEmitterConfig>();
                ParticleConfigs.Add("Fireball", _scene.content.Load<ParticleEmitterConfig>("particles/Sun"));
               // _particleConfigs.Add("Smokescreen", _scene.content.Load<ParticleEmitterConfig>("particles/BlueFlame"));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Entity CreateEntity(EntityType type, Vector2 location = new Vector2())
        {
            switch (type)
            {
                case EntityType.Ball:
                    var ball = _scene.createEntity("ball", location);
                    var ballTexture = _atlas.getSubtexture("ball");
                    var ballSprite = new Sprite(ballTexture);
                    ballSprite.setRenderLayer(2);
                    ball.addComponent(ballSprite);
                    var ballCollider = new CircleCollider(ballTexture.origin.X);
                    Flags.setFlag(ref ballCollider.physicsLayer, (int)CollisionLayer.Balls);
                    ball.addComponent(ballCollider);
                    ball.addComponent(new Mover());
                    ball.addComponent(new BallLogic(_scene.content.Load<SoundEffect>("sound/paddle-hit"),
                        _scene.content.Load<SoundEffect>("sound/ceiling-hit")));
                  
                    return ball;

                case EntityType.Player:
                    var paddle = _scene.createEntity("paddle", new Vector2(32, (float)Screen.height / 2));
                    var paddleTexture = _atlas.getSubtexture("paddle");
                    paddle.addComponent(new Sprite(paddleTexture));
                    paddle.addComponent(new BoxCollider(paddleTexture.sourceRect.Width, paddleTexture.sourceRect.Height));
                    paddle.addComponent(new Mover());
                    paddle.addComponent<PaddleContoller>();
                    
                    return paddle;

                case EntityType.CpuPlayer:
                    var cpuPaddle = _scene.createEntity("cpu", new Vector2(Screen.width - 32, (float)Screen.height / 2));
                    var sprite = new Sprite(_atlas.getSubtexture("paddle"))
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally
                    };
                    cpuPaddle.addComponent(sprite);
                    cpuPaddle.addComponent(new BoxCollider(sprite.width, sprite.height));
                    cpuPaddle.addComponent(new Mover());
                    cpuPaddle.addComponent<CpuLogic>();
                    return cpuPaddle;
                
                case EntityType.ItemSpawner:
                    var spawner = _scene.createEntity("item spawner", Screen.center);
                    spawner.addComponent(new ItemSpawner(ItemType.FireballPowerUp));
                    return spawner;

                case EntityType.FireballPowerUp:
                    var fireball = _scene.createEntity("fireball powerup");
                    fireball.addComponent(new PrototypeSprite(32, 32) { color = Color.OrangeRed });
                    var fireballCollider = new BoxCollider(32, 32);
                    Flags.setFlag(ref fireballCollider.physicsLayer, (int)CollisionLayer.Items);
                    Flags.setFlag(ref fireballCollider.collidesWithLayers, (int)CollisionLayer.Balls);
                    fireball.addComponent(new BoxCollider { isTrigger = true });
                    fireball.addComponent(new PickupLogic());
                    return fireball;

                //case EntityTypes.Smokescreen:
                //    var smokeScreen = _scene.createEntity("smokescreen", Screen.center);
                //    var p = new ParticleEmitter(_particleConfigs["Smokescreen"]);
                //    p.setRenderLayer(1);
                //    smokeScreen.addComponent(p);
                //    return smokeScreen;

                //case EntityTypes.Fireball:
                //    var fb = _scene.createEntity("FireballPowerUp", new Vector2(300));
                //    var a = Graphics.instance;
                //    PrototypeSprite ps = new PrototypeSprite(32, 32);
                //    ps.color = Color.OrangeRed;
                //    fb.addComponent(ps);

                //    var collider = new BoxCollider(ps.width, ps.height);
                //    collider.isTrigger = true;
                //    fb.addComponent(collider);

                //    fb.addComponent(new PickupLogic());
                //    return fb;


            }
            return null;
        }
    }
}
