using Microsoft.Xna.Framework;
using System.Linq;

namespace Coldsteel.Controls
{
    public class TextInputControl : Control<TextInputControlBinding>
    {
        public TextInputControl(string name) : base(name)
        {
        }

        public string InputBuffer(PlayerIndex playerIndex = PlayerIndex.One) =>
            _bindingsByPlayer[(int)playerIndex].FirstOrDefault()?.InputBuffer ?? "";
    }
}
