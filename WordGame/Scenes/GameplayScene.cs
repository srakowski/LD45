﻿using Coldsteel;
using Microsoft.Xna.Framework.Graphics;
using WordGame.Entities;
using WordGame.Logic;

namespace WordGame.Scenes
{
    class GameplayScene
    {
        internal static Scene Create(Gameplay gameplay)
        {
            var scene = new Scene();

            scene.AddAsset(new Asset<SpriteFont>(Constants.ProtoFont));

            SpriteLayers.Add(scene);

            scene.AddEntity(new EncounterController(gameplay));
            scene.AddEntity(new AvailableLettersBoard(gameplay));
            scene.AddEntity(new WordEntryBoard(gameplay));
            scene.AddEntity(new EnemyDisplay(gameplay));
            scene.AddEntity(new EnemyStatsDisplay(gameplay));
            scene.AddEntity(new PlayerDisplay(gameplay));
            scene.AddEntity(new PlayerStatsDisplay(gameplay));

            //scene.AddSpriteLayer(new SpriteLayer(SpriteLayers.Default));

            //var l = new Entity();
            //l.AddComponent(new TextSprite("ProtoFont", "Main Menu", SpriteLayers.Default));
            //scene.AddEntity(l);


            return scene;
        }
    }
}
