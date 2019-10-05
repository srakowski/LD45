using Coldsteel;
using Microsoft.Xna.Framework;
using WordGame.Behaviors;
using WordGame.Logic;
using static WordGame.Constants;

namespace WordGame.Entities
{
    public class EnemyDisplay : Entity
    {
        private readonly Gameplay gameplay;
        private readonly TextSprite textSprite;

        public EnemyDisplay(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            this.Position = new Vector2(1000, 200);

            textSprite = new TextSprite(ProtoFont, "_", SpriteLayers.UITop);
            AddComponent(textSprite);

            AddComponent(new RelayBehavior((Update)));
        }

        private void Update()
        {
            if (gameplay.ActiveEnemy.HasValue)
            {
                textSprite.Enabled = true;
                textSprite.Text = gameplay.ActiveEnemy.Select(a => a.Name).ValueOr(() => "");
            }
            else
            {
                textSprite.Enabled = false;
            }
        }
    }
}
