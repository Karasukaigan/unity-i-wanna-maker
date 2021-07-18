using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 水的类，通常被添加在判断玩家是出水还是入水的空物体（InWaterDetect和OutWaterDetect）上。
    /// </summary>
    public class Water : MonoBehaviour
    {
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        public bool isInWaterDetect = true;

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            var player = model.player;
            //if (collider.tag == "Player" && !player.inWater)
            if (collider.tag == "Player")
            {
                if (isInWaterDetect)
                {
                    //player.velocity.x = 0;
                    //player.velocity.y = 0;
                    player.inWater = true;
                    Debug.Log(player.inWater);
                }
                else if (!isInWaterDetect)
                {
                    player.inWater = false;
                    Debug.Log(player.inWater);
                }
                
            }
        }   

    }
}