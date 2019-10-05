using Coldsteel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordGame.Logic;

namespace WordGame
{
    public class WordGame : Game
    {
        GraphicsDeviceManager graphics;
        Engine engine;
        Words ws;

        public WordGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1440;

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
            ws = new Words();
            ws.Initialize();
            GameState.New(ws);
            engine.LoadScene("MainMenu");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
    }
}
