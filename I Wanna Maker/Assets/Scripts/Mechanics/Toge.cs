using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 陷阱的类，通常被添加在陷阱物体上。
    /// </summary>
    public class Toge : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider) 
        {
            //var p = collider.gameObject.GetComponent<PlayerController>();

            //如果刺碰到玩家则触发PlayerEnteredToge
            //if(p != null)
            if (collider.tag == "Player")
            {
                var ev = Schedule<PlayerEnteredToge>();
                ev.toge = this;
            }
        }
    }
}