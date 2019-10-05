using Coldsteel;
using Microsoft.Xna.Framework;
using WordGame.Logic;
using static WordGame.Constants;

namespace WordGame.Entities
{
    public class WordEntryBoard : Entity
    {
        private Gameplay gameplay;

        public WordEntryBoard(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            Position = new Vector2(350, 500);
            for (int c = 0; c < WordEntryMaxLetters; c++)
            {
                var wet = new WordEntryTile(
                    new Vector2(c * WordEntryTileWidth, 0),
                    gameplay,
                    c);
                AddChild(wet);
            }
        }
    }
}
