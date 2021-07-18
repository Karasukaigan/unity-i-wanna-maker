using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// 添加此脚本的物体在被玩家触碰后会被销毁，通常用于作为陷阱的地形方块。
    /// </summary>
    public class DisappearAfterTouch : MonoBehaviour
    {
        /// <summary>
        /// 是否可用。
        /// </summary>
        [Tooltip("是否可用。")]
        public bool isAvailable = true;

        /// <summary>
        /// 与玩家碰撞后销毁自身。
        /// </summary>
        /// <param name="other">玩家的碰撞器。</param>
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player" && isAvailable) Destroy(this.gameObject);
        }
    }
}

