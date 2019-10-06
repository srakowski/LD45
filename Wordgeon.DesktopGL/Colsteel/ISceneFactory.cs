// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Coldsteel
{
    public interface ISceneFactory
    {
        Scene Create(string sceneName, object param);
    }
}
