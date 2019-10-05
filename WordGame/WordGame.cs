using Coldsteel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WordGame.Logic;

namespace WordGame
{
    public class WordGame : Game
    {
        GraphicsDeviceManager graphics;
        Engine engine;
        Gameplay gameplay;

        public WordGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            engine = new Engine(this, new EngineConfig(
                new Scenes.SceneFactory(),
                Controls.Create()
            ));
        }

        protected override void Initialize()
        {
            base.Initialize();
            gameplay = new Gameplay();
            engine.LoadScene(nameof(Scenes.GameplayScene), gameplay);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
    }
}
