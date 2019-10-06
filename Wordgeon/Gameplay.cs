using Microsoft.Xna.Framework;
using System;
using Wordgeon.Logic;

namespace Wordgeon
{
    public class Gameplay
    {
        private Gameplay(GameState initialState)
        {
            State = initialState;
        }

        public GameState State { get; private set; }

        public Point DugeonLevelDim => new Point(Constants.LevelDim, Constants.LevelDim);

        public Player Player => State.Player;

        public Maybe<TilePlacer> TilePlacer => State.TilePlacer;

        public static Gameplay NewGame()
        {
            var random = new Random();
            var state = GameState.New(random);
            return new Gameplay(state);
        }

        internal Maybe<DungeonCell> GetDungeonCell(Point dungeonPos) => State.Dungeon.GetCell(dungeonPos);

        internal bool ActionPlayer(int colDelta, int rowDelta) =>
            HandleUpdateResult(State.ActionPlayer(colDelta, rowDelta));

        internal bool MoveTilePlacer(int colDelta, int rowDelta) =>
            HandleUpdateResult(State.MoveTilePlacer(colDelta, rowDelta));

        internal void StartTilePlacer() =>
            HandleUpdateResult(State.StartTilePlacer());

        internal bool CancelTilePlacer() =>
            HandleUpdateResult(State.CancelTilePlacer());

        internal bool ChangeTilePlacementDirection() =>
            HandleUpdateResult(State.ChangeTilePlacementDirection());

        private bool HandleUpdateResult(Maybe<GameState> result)
        {
            if (!result.HasValue)
            {
                return false;
            }

            State = result.Value;
            return true;
        }

        internal bool StartWordEntry() => HandleUpdateResult(State.StartWordEntry());
        internal bool WordEntryBackspace() => HandleUpdateResult(State.WordEntryBackspace());
        internal bool WordEntryLetter(char c) => HandleUpdateResult(State.WordEntryLetter(c));
        internal bool CommitWordEntry() => HandleUpdateResult(State.CommitWordEntry());
        internal bool CancelWordEntry() => HandleUpdateResult(State.CancelWordEntry());

        internal void AscendIfOnStair() => HandleUpdateResult(State.TakeStairs());
    }
}
