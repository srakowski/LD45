using Coldsteel;

namespace Wordgeon
{
    public static class SpriteLayers
    {
        public const string Default = "Default";
        public const string LetterTiles = "LetterTiles";
        public const string OccupantTiles = "OT";
        public const string Player = "Player";

        internal static void Setup(Scene scene)
        {
            scene.AddSpriteLayer(new SpriteLayer(Default));
            scene.AddSpriteLayer(new SpriteLayer(LetterTiles) { Depth = 10 });
            scene.AddSpriteLayer(new SpriteLayer(OccupantTiles) { Depth = 15 });
            scene.AddSpriteLayer(new SpriteLayer(Player) { Depth = 20 });
        }
    }
}
