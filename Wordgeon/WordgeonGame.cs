using Coldsteel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wordgeon
{
    public class WordgeonGame : Game
    {
        GraphicsDeviceManager graphics;
        Engine engine;
        Gameplay gameplay;

        public WordgeonGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1440;
            graphics.ApplyChanges();

            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            engine = new Engine(this, new EngineConfig(
                new SceneFactory(),
                Controls.Create()
            ));
        }

        protected override void Initialize()
        {
            base.Initialize();
            gameplay = Gameplay.NewGame();
            engine.LoadScene(nameof(SceneFactory.GameStartScene), gameplay);
        }
    }
}
