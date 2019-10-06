using System;
using System.Linq;
using Coldsteel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

            if (sceneName == nameof(GameStartScene))
            {
                return GameStartScene(param as Gameplay);
            }

            if (sceneName == nameof(GameOverScene))
            {
                return GameOverScene(param as Gameplay);
            }

            throw new NotImplementedException();
        }

        public Scene GameplayScene(Gameplay gameplay)
        {
            var scene = new Scene();

            scene.AddAsset(new Asset<Texture2D>("tile"));
            scene.AddAsset(new Asset<Texture2D>("player"));
            scene.AddAsset(new Asset<Texture2D>("tile_placer"));
            scene.AddAsset(new Asset<Texture2D>("lettertiles"));
            scene.AddAsset(new Asset<Texture2D>("crystals"));
            scene.AddAsset(new Asset<Texture2D>("stairs"));
            scene.AddAsset(new Asset<Texture2D>("rocks"));
            scene.AddAsset(new Asset<Texture2D>("btoy"));
            scene.AddAsset(new Asset<SpriteFont>("Font"));

            SpriteLayers.Setup(scene);

            scene.AddEntity(new GameplayControllerEntity(gameplay));
            scene.AddEntity(new DungeonLevelDisplay(gameplay));
            scene.AddEntity(new PlayerEntity(gameplay));
            scene.AddEntity(new TilePlacerEntity(gameplay));
            scene.AddEntity(new Hud(gameplay));

            return scene;
        }

        public Scene GameStartScene(Gameplay gameplay)
        {
            var scene = new Scene();

            scene.AddAsset(new Asset<SpriteFont>("Font"));
            SpriteLayers.Setup(scene);

            var text =
            @"Wordgeon Descent - The Search for the Blank Tile of Yendor

You are Kenney Letter, a Wizard of Letters and Words. You have
mastered converting crystals in to letter tiles which allow
you to traverse the lava dungeons of Wordgeon, that is, so long as
you arrange the the letter tiles into proper connected words.

Legend tells of a blank tile deep in the Wordgeon dungeon. This
so called Blank Tile o Yendor has the power to free letter tile
wizardry from having to arrage tiles in to proper words.

It is your quest to descend the lava dungeon of Wordgeon and find the
blank tile. Be careful to collect crystals as you go so that you don't
run out of letter tiles that would leave you stuck in the depths of
the dungeon.

Good luck!

Press [Enter] to start...
";

            var e = new Entity();
            e.Position = new Microsoft.Xna.Framework.Vector2(100, 300);
            e.AddComponent(new TextSprite("Font", text, SpriteLayers.Default));
            scene.AddEntity(e);
            e.AddComponent(new RelayBehavior(() =>
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    scene.Load(nameof(GameplayScene), gameplay);
            }));

            return scene;
        }

        public Scene GameOverScene(Gameplay gameplay)
        {
            var scene = new Scene();

            scene.AddAsset(new Asset<SpriteFont>("Font"));
            SpriteLayers.Setup(scene);

            scene.AddAsset(new Asset<Texture2D>("btoy"));

            var btoy = new Entity();
            btoy.Position = new Microsoft.Xna.Framework.Vector2(116, 200);
            btoy.AddComponent(new Sprite("btoy", SpriteLayers.Default) { Origin = new Microsoft.Xna.Framework.Vector2(45, 40)});
            scene.AddEntity(btoy);


            var e = new Entity();
            e.Position = new Microsoft.Xna.Framework.Vector2(100, 300);
            e.AddComponent(new TextSprite("Font", $"You found the Blank Tile of Yendor!\nYou win!\n\nScore: {gameplay.Player.LetterInventory.Count() * 1000}\n\nSee you next Ludum Dare!\n\nPress [Enter] to play again...", SpriteLayers.Default));

            e.AddComponent(new RelayBehavior(() =>
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    scene.Load(nameof(GameplayScene), Gameplay.NewGame());
            }));

            scene.AddEntity(e);


            return scene;
        }
    }
}
