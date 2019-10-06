using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Wordgeon.Logic
{
    public class GameState
    {
        private GameState(
            Dungeon dungeon,
            Player player,
            Maybe<TilePlacer> tilePlacer
            )
        {
            Dungeon = dungeon;
            Player = player;
            TilePlacer = tilePlacer;
        }

        public Dungeon Dungeon { get; }

        public Player Player { get; }

        public Maybe<TilePlacer> TilePlacer { get; }

        public static GameState New(Random random)
        {
            var dungeon = Dungeon.New(random);
            var player = Logic.Player.New();
            return new GameState(dungeon, player, Maybe.None<TilePlacer>());
        }

        internal Maybe<GameState> ActionPlayer(int colDelta, int rowDelta)
        {
            var actionTo = Player.LevelPosition + new Point(colDelta, rowDelta);
            var maybeCell = Dungeon.GetCell(actionTo);
            if (!maybeCell.HasValue) return Maybe.None<GameState>();

            var c = maybeCell.Value;

            if (!c.LetterTile.HasValue && !c.Occupant.HasValue)
            {

                return Maybe.None<GameState>();
            }

            if (c.Occupant.HasValue)
            {
                var occupant = c.Occupant.Value;
                if (occupant is DownStairs)
                {
                    var d = Dungeon.Descend();
                    if (!d.HasValue) return Maybe.None<GameState>();
                    return new GameState(
                        d.Value,
                        Player.SetPosition(c.Position),
                        TilePlacer
                        );
                }
                //}
                //else if (occupant is UpStairs)
                //{
                //    var d = Dungeon.Ascend();
                //    if (!d.HasValue) return Maybe.None<GameState>();
                //    return new GameState(
                //        d.Value,
                //        Player.SetPosition(c.Position),
                //        TilePlacer
                //        );
                //}
            }

            //if (maybeCell.HasValue && !maybeCell.Value.LetterTile.HasValue)
            //{
            //    return new GameState(
            //        Dungeon,
            //        Player,
            //        new TilePlacer(
            //            maybeCell.Value.Position,
            //            Player,
            //            colDelta != 0 ? TilePlacementDir.Row : TilePlacementDir.Col,
            //            Enumerable.Empty<Maybe<LetterTile>>()
            //        ));
            //}

            return maybeCell.Bind(cell =>
                Player
                    .Action(cell)
                    .Select(playerAndCell => new GameState(
                        Dungeon.ReplaceCell(playerAndCell.Item2),
                        playerAndCell.Item1,
                        TilePlacer
                    ))
                );
        }

        internal Maybe<GameState> TakeStairs()
        {
            var pcell = Dungeon.GetCell(Player.LevelPosition);
            if (pcell.Value.Occupant.HasValue)
            {
                var occupant = pcell.Value.Occupant.Value;
                //if (occupant is UpStairs)
                //{
                //    var d = Dungeon.Ascend();
                //    if (!d.HasValue) return Maybe.None<GameState>();
                //    return new GameState(
                //        d.Value,
                //        Player,
                //        TilePlacer
                //        );
                //}
                //else 
                if (occupant is DownStairs)
                {
                    var d = Dungeon.Descend();
                    if (!d.HasValue) return Maybe.None<GameState>();
                    return new GameState(
                        d.Value,
                        Player,
                        TilePlacer
                        );
                }
            }
            return Maybe.None<GameState>();
        }

        internal Maybe<GameState> StartTilePlacer()
        {
            return new GameState(
                Dungeon,
                Player,
                new TilePlacer(
                    Player.LevelPosition,
                    Player,
                    TilePlacementDir.Col,
                    Enumerable.Empty<Maybe<LetterTile>>()
                ));
        }

        internal Maybe<GameState> CancelTilePlacer()
        {
            return new GameState(
                Dungeon,
                Player,
                Maybe.None<TilePlacer>()
                );
        }

        internal Maybe<GameState> ChangeTilePlacementDirection()
        {
            return TilePlacer
                .Bind(tp => tp.ChangePlacementDirection())
                .Select(tp => new GameState(
                    Dungeon,
                    Player,
                    tp
                ));
        }

        internal Maybe<GameState> MoveTilePlacer(int colDelta, int rowDelta)
        {
            if (!TilePlacer.HasValue)
            {
                return Maybe.None<GameState>();
            }

            var actionTo = TilePlacer.Value.LevelPosition + new Point(colDelta, rowDelta);
            var maybeCell = Dungeon.GetCell(actionTo);

            return maybeCell.Bind(cell =>
                TilePlacer
                    .Value
                    .Move(cell)
                    .Select(tilePlacer => new GameState(
                        Dungeon,
                        Player,
                        tilePlacer
                    ))
                );
        }

        internal Maybe<GameState> StartWordEntry()
        {
            var dungeon = Dungeon.SaveActiveLevel();
            return new GameState(
                dungeon,
                Player,
                TilePlacer
                );
        }

        internal Maybe<GameState> WordEntryBackspace()
        {
            if (!TilePlacer.HasValue) return Maybe.None<GameState>();
            var tp = TilePlacer.Value;

            var player = Player;
            var lt = tp.WordEntry.LastOrDefault();
            if (lt.HasValue)
            {
                player = Player.AddLetterTiles(new[] { lt.Value });
            }

            tp = tp.RegressWord();

            var dg = Dungeon.ToMaybe();
            if (lt.HasValue)
            {
                dg = dg.Value.RemoveLetterTile(tp.LevelPosition);
                if (!dg.HasValue) return Maybe.None<GameState>();
            }

            return new GameState(
                dg.Value,
                player,
                tp
                );
        }

        internal Maybe<GameState> CommitWordEntry()
        {
            if (!TilePlacer.HasValue) return Maybe.None<GameState>();
            var tp = TilePlacer.Value;

            if (!Dungeon.AllLetterTilesAreAdjacent())
                return Maybe.None<GameState>();

            var allWords = Dungeon.CollectAllWords();
            if (!allWords.All(GameDictionary.HasWord))
                return Maybe.None<GameState>();

            return new GameState(
                Dungeon,
                Player,
                Maybe.None<TilePlacer>()
                );
        }

        internal Maybe<GameState> CancelWordEntry()
        {
            if (!TilePlacer.HasValue) return Maybe.None<GameState>();
            var tp = TilePlacer.Value;

            var dungeon = Dungeon.RestoreActiveLevel();

            var player = Player.AddLetterTiles(tp.WordEntry.Where(w => w.HasValue).Select(w => w.Value));

            tp = tp.FlushLetterTiles();

            return new GameState(
                dungeon,
                player,
                tp
                );
        }

        internal Maybe<GameState> WordEntryLetter(char letter)
        {
            if (!TilePlacer.HasValue) return Maybe.None<GameState>();

            var tp = TilePlacer.Value;

            var pos = tp.LevelPosition;
            var maybeCell = Dungeon.GetCell(pos);

            if (!maybeCell.HasValue) return Maybe.None<GameState>();
            var cell = maybeCell.Value;

            if (cell.Occupant.HasValue)
            {
                return Maybe.None<GameState>();
            }

            if (cell.LetterTile.HasValue)
            {
                //if (cell.LetterTile.Value.Value != letter)
                //    return Maybe.None<GameState>();

                tp = tp.AdvanceWord(Maybe.None<LetterTile>());
                return new GameState(
                    Dungeon,
                    Player,
                    tp);
            }

            var player = Player.RemoveLetter(letter);
            if (!player.HasValue)
            {
                return Maybe.None<GameState>();
            }

            var lt = new LetterTile(letter);

            var dg = Dungeon.PlaceLetterTile(tp.LevelPosition, lt);
            if (!dg.HasValue)
            {
                return Maybe.None<GameState>();
            }

            tp = tp.AdvanceWord(lt);

            return new GameState(
                dg.Value,
                player.Value,
                tp);
        }
    }
}
