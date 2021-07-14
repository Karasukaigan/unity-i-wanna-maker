using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    public class Portal : MonoBehaviour
    {
        public GameObject targetPortal; //Connected portal
        private PlatformerModel model = Simulation.GetModel<PlatformerModel>(); //Get PlatformerModel

        public float coolingTime = 1f; 
        public float portalTimer = 0f; 
        public bool isOpen = true;

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            if (isOpen && portalTimer <= 0 && targetPortal != null)
            {
                var p = targetPortal.gameObject.GetComponent<Portal>();
                var player = model.player;

                if(collider.tag == "Player")
                {
                    player.Teleport(targetPortal.transform.position); //Teleport to the target portal
                }
                portalTimer = coolingTime; 
                p.portalTimer = coolingTime; 
            }
        }
        
        protected void Update()
        {
            if (portalTimer > 0) portalTimer -= Time.deltaTime;
        }
    }
}