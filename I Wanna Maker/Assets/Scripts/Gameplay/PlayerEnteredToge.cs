using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a player enters a trigger with a Toge component.
    /// </summary>
    /// <typeparam name="PlayerEnteredDeathZone"></typeparam>
    public class PlayerEnteredToge : Simulation.Event<PlayerEnteredToge>
    {
        public Toge toge;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            Simulation.Schedule<PlayerDeath>(0);
        }
    }
}
