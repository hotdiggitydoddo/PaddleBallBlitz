#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using PaddleBallBlitz.Shared.Scenes;

#endregion

namespace PaddleBallBlitz.Shared
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PaddleBallBlitzGame : Core
    {
        private GameScene _gameScene;

        protected override void Initialize()
        {
            base.Initialize();

            _gameScene = new GameScene();
            scene = _gameScene;
        }
    }
        
}
