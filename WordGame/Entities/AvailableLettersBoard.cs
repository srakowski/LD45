using Coldsteel;
using Microsoft.Xna.Framework;
using WordGame.Logic;
using static WordGame.Constants;

namespace WordGame.Entities
{

    public class AvailableLettersBoard : Entity
    {
        private Gameplay gameplay;

        public AvailableLettersBoard(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            Position = new Vector2(600, 600);
            int i = 0;
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    var alt = new AvailableLetterTile(
                        new Vector2(c * AvailableTileWidth, r * AvailableTileHeight),
                        gameplay,
                        i);
                    AddChild(alt);
                    i++;
                }
            }
        }
    }
}
