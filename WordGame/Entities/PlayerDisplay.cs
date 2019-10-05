using Coldsteel;
using Microsoft.Xna.Framework;
using WordGame.Logic;
using static WordGame.Constants;

namespace WordGame.Entities
{
    public class PlayerDisplay : Entity
    {
        private readonly Gameplay gameplay;
        private readonly TextSprite textSprite;

        public PlayerDisplay(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            this.Position = new Vector2(100, 200);

            textSprite = new TextSprite(ProtoFont, gameplay.Player.Name, SpriteLayers.UITop);
            AddComponent(textSprite);

            //AddComponent(new RelayBehavior((Update)));
        }
    }
}
