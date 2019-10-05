using Coldsteel;
using WordGame.Logic;

namespace WordGame.Scenes
{
    public class SceneFactory : ISceneFactory
    {
        public Scene Create(string sceneName, object param)
        {
            if (sceneName == "MainMenu")
            {
                return MainMenuScene.Create(param as GameState);
            }
            return null;
        }
    }
}
