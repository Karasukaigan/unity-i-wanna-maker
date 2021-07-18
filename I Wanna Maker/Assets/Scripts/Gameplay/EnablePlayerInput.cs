using Platformer.Core;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 当应启用用户输入时触发此事件，已弃用。
    /// </summary>
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            //var player = model.player;
            //player.controlEnabled = true;
            //player.Show();
        }
    }
}