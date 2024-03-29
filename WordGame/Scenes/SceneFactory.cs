﻿using Coldsteel;
using System;
using WordGame.Logic;

namespace WordGame.Scenes
{
    public class SceneFactory : ISceneFactory
    {
        public Scene Create(string sceneName, object param)
        {
            if (sceneName == nameof(MainMenuScene))
            {
                return MainMenuScene.Create(param as Gameplay);
            }
            else if (sceneName == nameof(GameplayScene))
            {
                return GameplayScene.Create(param as Gameplay);
            }
            throw new NotImplementedException();
        }
    }
}
