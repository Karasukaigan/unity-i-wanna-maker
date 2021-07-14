using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    public class Toge : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider) 
        {
            //If the trap hits the player
            //if(p != null)
            if (collider.tag == "Player")
            {
                var ev = Schedule<PlayerEnteredToge>();
                ev.toge = this;

                //Destroy(collider.gameObject); //Delete object

                //SceneManager.LoadScene ("MainScene"); //Overload scene
            }
        }
    }
}