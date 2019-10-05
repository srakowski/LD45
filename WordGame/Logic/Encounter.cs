using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGame.Logic
{
    public class Encounter
    {
        public Encounter(
            Maybe<Enemy> activeEnemy,
            IEnumerable<Enemy> enemyQueue
            )
        {
            ActiveEnemy = activeEnemy;
            EnemyQueue = enemyQueue;
        }

        public Maybe<Enemy> ActiveEnemy { get; }

        public IEnumerable<Enemy> EnemyQueue { get; }

        public bool HasLivingEnemies => ActiveEnemy.HasValue || EnemyQueue.Any();

        public Encounter TakeDamage(int points)
        {
            if (!ActiveEnemy.HasValue)
            {
                return this;
            }

            var ae = ActiveEnemy.Value;
            var dae = ae.TakeDamage(points);
            return new Encounter(dae, EnemyQueue);
        }

        public Encounter TakePartialDamage(int points)
        {
            var effectivePoints = ActiveEnemy
                .Select(e => Math.Clamp(points - e.AttackDefense, 0, int.MaxValue))
                .ValueOr(() => points);
            return TakeDamage(effectivePoints);
        }

        public Maybe<Encounter> LoadNextEnemy()
        {
            if (!HasLivingEnemies || ActiveEnemy.HasValue)
            {
                return Maybe.None<Encounter>();
            }

            return new Encounter(
                EnemyQueue.First().ToMaybe(),
                EnemyQueue.Skip(1).ToArray()
                ).ToMaybe();
        }
    }
}
