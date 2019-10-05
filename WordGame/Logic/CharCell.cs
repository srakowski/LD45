using System;

namespace WordGame.Logic
{
    public class CharCell
    {
        public CharCell(char value, int combatValue, Maybe<int> selectionIndex)
        {
            Value = value;
            CombatValue = combatValue;
            SelectionIndex = selectionIndex;
        }

        public char Value { get; }

        public int CombatValue { get; }

        public Maybe<int> SelectionIndex { get; }

        public bool IsSelected => SelectionIndex.HasValue;

        public CharCell Select(int index)
        {
            return new CharCell(Value, CombatValue, Maybe.Some<int>(index));
        }

        public CharCell Deselect()
        {
            return new CharCell(Value, CombatValue, Maybe.None<int>());
        }
    }
}
