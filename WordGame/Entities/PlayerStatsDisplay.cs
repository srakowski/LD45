using Coldsteel;
using Microsoft.Xna.Framework;
using WordGame.Behaviors;
using WordGame.Logic;

namespace WordGame.Entities
{
    public class PlayerStatsDisplay : Entity
    {
        private readonly Gameplay gameplay;
        private readonly TextSprite textSprite;

        public PlayerStatsDisplay(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            this.Position = new Vector2(100, 300);

            textSprite = new TextSprite(Constants.ProtoFont, "_", SpriteLayers.UITop);
            AddComponent(textSprite);

            AddComponent(new RelayBehavior((Update)));
        }

        private void Update()
        {
            var player = gameplay.Player;
            textSprite.Text =
                $"HP: {player.HP}\n";
        }
    }
}
