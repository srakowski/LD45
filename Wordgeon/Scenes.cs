using System;
using Coldsteel;
using Microsoft.Xna.Framework.Graphics;

namespace Wordgeon
{
    public class SceneFactory : ISceneFactory
    {
        public Scene Create(string sceneName, object param)
        {
            if (sceneName == nameof(GameplayScene))
            {
                return GameplayScene(param as Gameplay);
            }
            throw new NotImplementedException();
        }

        public Scene GameplayScene(Gameplay gameplay)
        {
            var scene = new Scene();

            scene.AddAsset(new Asset<Texture2D>("tile"));
            scene.AddAsset(new Asset<Texture2D>("player"));
            scene.AddAsset(new Asset<Texture2D>("tile_placer"));
            scene.AddAsset(new Asset<SpriteFont>("Font"));

            SpriteLayers.Setup(scene);

            scene.AddEntity(new DungeonLevelDisplay(gameplay));
            scene.AddEntity(new PlayerEntity(gameplay));
            scene.AddEntity(new GameplayControllerEntity(gameplay));
            scene.AddEntity(new TilePlacerEntity(gameplay));

            return scene;
        }
    }
}
