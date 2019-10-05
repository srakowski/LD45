
using Coldsteel;
using WordGame.Logic;

namespace WordGame.Behaviors
{
    class UpdateWordEntryTile : Behavior
    {
        private readonly TextSprite sprite;
        private readonly Gameplay gameplay;
        private readonly int index;

        public UpdateWordEntryTile(TextSprite sprite, Gameplay gameplay, int index)
        {
            this.sprite = sprite;
            this.gameplay = gameplay;
            this.index = index;
        }

        protected override void Initialize()
        {
            UpdateText();
        }

        protected override void Update()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (index < 2)
            {
                sprite.Text = gameplay.CurrentState
                    .StartsWith.Value[index]
                    .ToString()
                    .ToUpper();
            }
            else
            {
                sprite.Text = gameplay.CurrentState.CharBoard
                    .GetSelectionAtWordIndex(index)
                    .Select(v => v.Value)
                    .ValueOr(() => '_')
                    .ToString()
                    .ToUpper();
            }
        }
    }
}
