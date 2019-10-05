using Microsoft.Xna.Framework;

namespace Coldsteel.Controls
{
    public abstract class TextInputControlBinding : ControlBinding
    {
        public TextInputControlBinding(PlayerIndex playerIndex) : base(playerIndex)
        {
        }

        public abstract string InputBuffer { get; }
    }
}