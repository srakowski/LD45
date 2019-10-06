using System.Collections.Generic;

namespace Wordgeon.Logic
{
    public interface IOccupant
    {
        Player InteractWithPlayer(Player player);
    }

    public class LetterChest : IOccupant
    {
        public LetterChest(IEnumerable<LetterTile> letterTiles)
        {
            LetterTiles = letterTiles; ;
        }

        public IEnumerable<LetterTile> LetterTiles { get; }

        public Player InteractWithPlayer(Player player)
        {
            return player.AddLetterTiles(this.LetterTiles);
        }
    }

    public class DownStairs : IOccupant
    {
        public Player InteractWithPlayer(Player player) => player;
    }

    public class UpStairs : IOccupant
    {
        public Player InteractWithPlayer(Player player) => player;
    }

    public class BlankTileOfYendor : IOccupant
    {
        public Player InteractWithPlayer(Player player)
        {
            return player.SetBlankTileOfYendor(player, this);
        }
    }
}
