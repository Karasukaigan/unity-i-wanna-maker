using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 该组件用于创建巡逻路径，敌人将在两个点之间移动。
    /// 这个类来自平台游戏Microgame模板。未使用。
    /// </summary>
    public partial class PatrolPath : MonoBehaviour
    {
        /// <summary>
        /// 巡逻路线的起始坐标和结束坐标。
        /// </summary>
        public Vector2 startPosition, endPosition;

        /// <summary>
        /// 创建一个Mover实例，用于以特定速度沿路径移动实体。
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public Mover CreateMover(float speed = 1) => new Mover(this, speed);

        void Reset()
        {
            startPosition = Vector3.left;
            endPosition = Vector3.right;
        }
    }
}