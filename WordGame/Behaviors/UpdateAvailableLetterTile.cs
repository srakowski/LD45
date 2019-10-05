
using Coldsteel;
using System.Linq;
using WordGame.Logic;

namespace WordGame.Behaviors
{
    class UpdateAvailableLetterTile : Behavior
    {
        private readonly TextSprite sprite;
        private readonly Gameplay gameplay;
        private readonly int index;

        public UpdateAvailableLetterTile(TextSprite sprite, Gameplay gameplay, int index)
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
            sprite.Text = gameplay.CurrentState.CharBoard
                .CharCells
                .Select(c => c.IsSelected ? '_' : c.Value)
                .ElementAt(index)
                .ToString()
                .ToUpper();
        }
    }
}
