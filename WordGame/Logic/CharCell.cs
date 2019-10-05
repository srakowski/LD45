using System;

namespace WordGame.Logic
{
    public class CharCell
    {
        public CharCell(char value, Maybe<int> selectionIndex, Maybe<Item> item)
        {
            Value = value;
            SelectionIndex = selectionIndex;
            Item = item;
        }

        public char Value { get; }

        public Maybe<int> SelectionIndex { get; }

        public bool IsSelected => SelectionIndex.HasValue;

        public Maybe<Item> Item { get; }

        public CharCell Select(int index)
        {
            return new CharCell(Value, Maybe.Some<int>(index), Item);
        }

        public CharCell Deselect()
        {
            return new CharCell(Value, Maybe.None<int>(), Item);
        }

        public CharCell WithLoot(Random random, Encounter value)
        {
            var item = Logic.Item.CreateLoot(random);
            return new CharCell(
                Value,
                SelectionIndex,
                item
                );
        }
    }
}
