using UnityEngine;

namespace Platformer.View
{
    /// <summary>
    /// 用于在应用了比例因子的情况下相对于主摄像机位置移动变换。这用于在游戏对象的不同分支上实现视差滚动效果。
    /// 这个类来自平台游戏Microgame模板。
    /// </summary>
    public class ParallaxLayer : MonoBehaviour
    {
        /// <summary>
        /// 图层的移动按此值缩放。
        /// </summary>
        public Vector3 movementScale = Vector3.one;

        Transform _camera;

        void Awake()
        {
            _camera = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.position = Vector3.Scale(_camera.position, movementScale);
        }

    }
}