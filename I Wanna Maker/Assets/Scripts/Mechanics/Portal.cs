using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 实现对玩家控制的类。它是AnimationController类的超集。
    /// </summary>
    public class Portal : MonoBehaviour
    {
        [Tooltip("相连接的传送门。")]
        public GameObject targetPortal;
        private PlatformerModel model = Simulation.GetModel<PlatformerModel>(); //获取PlatformerModel

        [Tooltip("传送冷却时间。")]
        public float coolingTime = 1f; 
        [Tooltip("传送冷却时间计时器。")]
        public float portalTimer = 0f; 
        [Tooltip("是否开放。")]
        public bool isOpen = true;

        /// <summary>
        /// 当与玩家发生碰撞时，将玩家传送到目标传送门，并进入传送冷却。
        /// </summary>
        /// <param name="collision">玩家的碰撞器。</param>
        private void OnTriggerEnter2D(Collider2D collider) 
        {
            if (isOpen && portalTimer <= 0 && targetPortal != null)
            {
                var p = targetPortal.gameObject.GetComponent<Portal>();
                var player = model.player;

                if(collider.tag == "Player")
                {
                    player.Teleport(targetPortal.transform.position);
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