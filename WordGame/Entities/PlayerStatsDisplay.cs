using Coldsteel;
using Microsoft.Xna.Framework;
using System.Linq;
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
                $"HP:  {player.HP}/{player.MaxHP}\n" +
                $"OFF: {player.Off}\n" +
                $"DEF: {player.Def}\n" +
                $"\n" +
                $"Inventory:" +
                $"\n- {string.Join("\n- ", player.Inventory.Select(i => i.Name))}";
        }
    }
}
