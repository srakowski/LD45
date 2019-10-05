using Coldsteel;
using WordGame.Behaviors;
using WordGame.Logic;

namespace WordGame.Entities
{
    public class EncounterController : Entity
    {
        private Gameplay gameplay;
        private TextSprite textSprite;

        public EncounterController(Gameplay gameplay)
        {
            this.gameplay = gameplay;

            textSprite = new TextSprite(Constants.ProtoFont, "BEGIN", SpriteLayers.UITop);
            AddComponent(textSprite);

            AddComponent(new EncounterGameplay(gameplay, textSprite));
        }
    }
}
