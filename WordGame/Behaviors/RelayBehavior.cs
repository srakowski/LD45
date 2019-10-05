using Coldsteel;
using System;

namespace WordGame.Behaviors
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
