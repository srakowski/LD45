using Coldsteel;
using System;

namespace Wordgeon
{
    public class RelayBehavior : Behavior
    {
        Action onUpdate;

        public RelayBehavior(Action onUpdate)
        {
            this.onUpdate = onUpdate;
        }

        protected override void Update() => onUpdate();
    }
}
