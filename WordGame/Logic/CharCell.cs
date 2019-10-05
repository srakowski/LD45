using System;

namespace WordGame.Logic
{
    public class CharCell
    {
        public CharCell(char value, Maybe<int> selectionIndex)
        {
            Value = value;
            SelectionIndex = selectionIndex;
        }

        public char Value { get; }

        public Maybe<int> SelectionIndex { get; }

        public bool IsSelected => SelectionIndex.HasValue;

        public CharCell Select(int index)
        {
            return new CharCell(Value, Maybe.Some<int>(index));
        }

        public CharCell Deselect()
        {
            return new CharCell(Value, Maybe.None<int>());
        }
    }
}
