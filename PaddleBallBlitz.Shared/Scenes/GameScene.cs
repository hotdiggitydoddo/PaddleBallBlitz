using Microsoft.Xna.Framework;
using Nez;
using PaddleBallBlitz.Components;
using PaddleBallBlitz.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaddleBallBlitz.Shared.Scenes
{
    public class GameScene : Scene
    {
        public override void initialize()
        {
            base.initialize();

            clearColor = Color.Black;
            addRenderer(new DefaultRenderer());

            addEntityProcessor(new ItemSpawnerSystem(new Matcher().all(typeof(ItemSpawner))));

            EntityFactory.Instance.Initialize(this);

            EntityFactory.Instance.CreateEntity(EntityType.Player);
            EntityFactory.Instance.CreateEntity(EntityType.CpuPlayer);
            EntityFactory.Instance.CreateEntity(EntityType.Ball);
            EntityFactory.Instance.CreateEntity(EntityType.ItemSpawner);
        }
    }
}
