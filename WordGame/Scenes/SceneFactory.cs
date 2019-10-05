using Coldsteel;

namespace WordGame.Scenes
{
    public class SceneFactory : ISceneFactory
    {
        public Scene Create(string sceneName)
        {
            if (sceneName == "MainMenu")
            {
                return MainMenuScene.Create();
            }
            return null;
        }
    }
}
