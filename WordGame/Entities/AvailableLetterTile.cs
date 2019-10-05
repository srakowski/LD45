using Coldsteel;
using Microsoft.Xna.Framework;
using WordGame.Behaviors;
using WordGame.Logic;
using static WordGame.Constants;

namespace WordGame.Entities
{
    public class AvailableLetterTile : Entity
    {
        private readonly Gameplay gameplay;
        private readonly int index;

        public AvailableLetterTile(Vector2 position, Gameplay gameplay, int index)
        {
            Position = position;
            this.gameplay = gameplay;
            this.index = index;

            // AddComponent(new Sprite("tiles", SpriteLayers.UITop));
            TextSprite = new TextSprite(ProtoFont, "N", SpriteLayers.UITop);
            AddComponent(TextSprite);
            AddComponent(new UpdateAvailableLetterTile(TextSprite, gameplay, index));
        }

        public TextSprite TextSprite { get; }
    }
}
