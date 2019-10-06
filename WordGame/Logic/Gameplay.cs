using System;
using System.Collections.Generic;

namespace WordGame.Logic
{
    public class Gameplay
    {
        public Gameplay()
        {
            Words = new Words();
            Words.Initialize();
            CurrentState = GameState.New(Words, "Noob");
        }

        public Words Words { get; }

        public GameState CurrentState { get; private set; }

        public bool EncounterIsActive => CurrentState.EncounterIsActive;

        public Player Player => CurrentState.Player;

        public bool EnemyIsAlive => CurrentState.EnemyIsAlive;

        public bool PlayerIsAlive => CurrentState.PlayerIsAlive;

        public Maybe<Enemy> ActiveEnemy => CurrentState.ActiveEnemy;

        internal bool MakeAutoLetterSelection(char c)
        {
            var result = CurrentState.MakeAutoLetterSelection(c);
            return HandleUpdateResult(result);
        }

        internal bool Attack()
        {
            var result = CurrentState.CompleteWord(CombatMode.Attack);
            return HandleUpdateResult(result);
        }

        internal bool Defend()
        {
            var result = CurrentState.CompleteWord(CombatMode.Defense);
            return HandleUpdateResult(result);
        }

        internal bool UndoLastSelection()
        {
            var result = CurrentState.UndoLastSelection();
            return HandleUpdateResult(result);
        }

        public bool LoadNextEnemy()
        {
            var result = CurrentState.LoadNextEnemy();
            return HandleUpdateResult(result);
        }

        public bool ExecuteEnemyTurn()
        {
            var result = CurrentState.ExecuteEnemyTurn();
            return HandleUpdateResult(result);
        }

        internal bool CollectLoot()
        {
            var result = CurrentState.CollectWinnings();
            return HandleUpdateResult(result);
        }

        public bool Fail()
        {
            return true;
        }

        private bool HandleUpdateResult(Maybe<GameState> result)
        {
            if (!result.HasValue)
            {
                return false;
            }

            CurrentState = result.Value;
            return true;
        }
    }
}
