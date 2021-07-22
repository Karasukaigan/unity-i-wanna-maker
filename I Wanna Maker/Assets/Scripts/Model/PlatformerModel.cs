using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Model
{
    /// <summary>
    /// 包含实现平台风格游戏所需数据的主模型。这个类应该只包含数据和对数据进行操作的方法。它使用GameController类中的数据进行初始化。
    /// 这个类来自平台游戏Microgame模板。
    /// </summary>
    [System.Serializable]
    public class PlatformerModel
    {
        /// <summary>
        /// 场景中的虚拟摄像机。
        /// </summary>
        [Tooltip("虚拟摄像机，可留空。")]
        public Cinemachine.CinemachineVirtualCamera virtualCamera;

        /// <summary>
        /// 玩家，通常为Player。
        /// </summary>
        [Tooltip("玩家，通常为Player。")]
        public PlayerController player;

        /// <summary>
        /// 出生点，通常为SpawnPoint。
        /// </summary>
        [Tooltip("出生点，通常为SpawnPoint。")]
        public Transform spawnPoint;

        /// <summary>
        /// 应用于所有初始跳跃速度的全局跳跃系数，推荐值为0.47。
        /// </summary>
        [Tooltip("应用于所有初始跳跃速度的全局跳跃修改器，推荐值为0.47。")]
        public float jumpModifier = 0.47f;

        /// <summary>
        /// 松开跳跃键时的跳跃减缓系数，推荐值为0.2。
        /// </summary>
        [Tooltip("松开跳跃键时的跳跃减缓系数，推荐值为0.2。")]
        public float jumpDeceleration = 0.2f;
    }
}