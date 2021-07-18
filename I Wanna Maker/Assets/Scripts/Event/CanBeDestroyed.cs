using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// 添加此脚本的物体可以被摧毁，通常用于可摧毁的陷阱或者地形方块。
    /// </summary>
    public class CanBeDestroyed : MonoBehaviour
    {
        /// <summary>
        /// 被子弹击中后销毁自身。当收到“BeShot”消息时触发。
        /// </summary>
        /// <param name="bulletDamage">子弹的伤害。</param>
        private void BeShot(int bulletDamage) 
        {
            Destroy(this.gameObject);
        }
    }
}