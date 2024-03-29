﻿using Coldsteel;
using Microsoft.Xna.Framework.Graphics;
using WordGame.Logic;

namespace WordGame.Scenes
{
    class MainMenuScene
    {
        internal static Scene Create(Gameplay gp)
        {
            var scene = new Scene();

            scene.AddAsset(new Asset<SpriteFont>("ProtoFont"));


            scene.AddSpriteLayer(new SpriteLayer(SpriteLayers.Default));

            var l = new Entity();
            l.AddComponent(new TextSprite("ProtoFont", "Main Menu", SpriteLayers.Default));
            scene.AddEntity(l);


            return scene;
        }
    }
}
