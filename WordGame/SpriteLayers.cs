using System;
using Coldsteel;

namespace WordGame
{
    static class SpriteLayers
    {
        public static string Default = "Default";
        public static string UITop = "UITop";

        internal static void Add(Scene scene)
        {
            scene.AddSpriteLayer(new SpriteLayer(Default));
            scene.AddSpriteLayer(new SpriteLayer(UITop));
        }
    }
}
