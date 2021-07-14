using UnityEngine;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{

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