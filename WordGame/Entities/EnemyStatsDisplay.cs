using Coldsteel;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using WordGame.Behaviors;
using WordGame.Logic;

namespace WordGame.Entities
{
    public class EnemyStatsDisplay : Entity
    {
        private readonly Gameplay gameplay;
        private readonly TextSprite textSprite;

        public EnemyStatsDisplay(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            this.Position = new Vector2(1000, 300);

            textSprite = new TextSprite(Constants.ProtoFont, "_", SpriteLayers.UITop);
            AddComponent(textSprite);

            AddComponent(new RelayBehavior((Update)));
        }

        private void Update()
        {
            if (gameplay.ActiveEnemy.HasValue)
            {
                textSprite.Enabled = true;
                var enemy = gameplay.ActiveEnemy.Value;
                textSprite.Text =
                    $"HP: {enemy.HP}/{enemy.MaxHP}\n" +
                    $"OFF: {enemy.Off}\n" +
                    $"DEF: {enemy.Def}\n";
            }
            else
            {
                textSprite.Enabled = false;
            }
        }
    }
}
