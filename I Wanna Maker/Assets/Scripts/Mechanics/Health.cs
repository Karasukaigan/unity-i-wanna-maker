using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 用来进行生命值相关操作的类。
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// 最大HP。
        /// </summary>
        [Tooltip("最大HP。")]
        public int maxHP = 1;

        /// <summary>
        /// 是否活着。
        /// </summary>
        public bool IsAlive => currentHP > 0;

        /// <summary>
        /// 当前生命值。
        /// </summary>
        int currentHP;

        /// <summary>
        /// 增加1点生命值。
        /// </summary>
        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP); //将HP的范围限制在0~maxHP，并+1
        }

        /// <summary>
        ///减少1点生命值，若当前HP为0时，触发HealthIsZero事件。
        /// </summary>
        public void Decrement()
        {
            currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
            if (currentHP == 0)
            {
                //HealthIsZero已弃用
                //var ev = Schedule<HealthIsZero>();
                //ev.health = this;
            }
        }

        /// <summary>
        /// 将生命值降为0，使用循环调用Decrement()来实现。
        /// </summary>
        public void Die()
        {
            while (currentHP > 0) Decrement();
        }

        /// <summary>
        /// 初始化。将当前生命值等于最大生命值。
        /// </summary>
        void Awake()
        {
            currentHP = maxHP;
        }
    }
}
