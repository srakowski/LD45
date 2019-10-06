using Microsoft.Xna.Framework;

namespace Coldsteel.Controls
{
    public class WindowTextInputControlBinding : TextInputControlBinding
    {
        public WindowTextInputControlBinding(PlayerIndex playerIndex) : base(playerIndex)
        {
        }

        public override string InputBuffer => InputManager.InputBuffer;
    }
}
