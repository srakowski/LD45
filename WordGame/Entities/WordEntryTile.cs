using Coldsteel;
using Microsoft.Xna.Framework;
using WordGame.Behaviors;
using WordGame.Logic;
using static WordGame.Constants;

namespace WordGame.Entities
{
    class WordEntryTile : Entity
    {
        private readonly Gameplay gameplay;
        private readonly int index;

        public WordEntryTile(
            Vector2 position,
            Gameplay gameplay,
            int index)
        {
            Position = position;
            this.gameplay = gameplay;
            this.index = index;

            // AddComponent(new Sprite("tiles", SpriteLayers.UITop));
            TextSprite = new TextSprite(ProtoFont, "_", SpriteLayers.UITop);
            AddComponent(TextSprite);

            AddComponent(new UpdateWordEntryTile(TextSprite, gameplay, index));
        }

        public TextSprite TextSprite { get; }
    }
}
