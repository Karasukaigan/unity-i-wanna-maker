using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 当玩家触碰到任何陷阱时触发。
    /// 若要使用，需要给陷阱添加Toge脚本。
    /// </summary>
    /// <typeparam name="PlayerEnteredToge"></typeparam>
    public class PlayerEnteredToge : Simulation.Event<PlayerEnteredToge>
    {
        public Toge toge;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            Simulation.Schedule<PlayerDeath>(0); //玩家触碰陷阱时死亡
        }
    }
}
