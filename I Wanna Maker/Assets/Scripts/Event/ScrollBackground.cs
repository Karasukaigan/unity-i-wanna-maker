using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// 实现了滚动背景的效果。可以自定义方向以及滚动速度。
    /// 建议滚动背景的长度要大于滚动方向上摄像机范围长度的四倍，且中间位置要能够和两端的图案衔接。
    /// </summary>
    public class ScrollBackground : MonoBehaviour
    {
        //滚动背景的起始坐标
        private float startX;
        private float startY;

        private float length; //滚动长度

        [Tooltip("滚动速度。")]
        public float speed = 10f;

        //滚动方向设置
        [Tooltip("横向滚动，配合isRight变量使用。")]
        public bool isHorizontal = false;
        [Tooltip("向右滚动。")]
        public bool isRight = false;
        [Tooltip("垂直滚动，配合isDown变量使用。")]
        public bool isVertical = true;
        [Tooltip("向下滚动。")]
        public bool isDown = true;
        
        /// <summary>
        /// 当前坐标。
        /// </summary>
        private float currentPosition = 0f;

        private void Start() {
            startX = transform.position.x;
            startY = transform.position.y;

            //截取滚动背景滚动方向上的长度的二分之一为实际的滚动长度
            if (isHorizontal) length = transform.localScale.x * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
            else if (isVertical) length = transform.localScale.y * GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2;
        }

        void Update()
        {
            //根据设置好的方向，改变滚动背景的坐标
            if (isHorizontal)
            {
                if (isRight) transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y, 0f);
                else transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, 0f);
            }
            else if (isVertical)
            {
                if (isDown) transform.position = new Vector3(transform.position.x, transform.position.y - (speed * Time.deltaTime), 0f);
                else transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), 0f);
            }

            //根据速度更新当前坐标
            currentPosition += (speed * Time.deltaTime);

            //当前坐标与起始坐标之间的距离大于滚动长度时回滚
            if (currentPosition > length) 
            {
                transform.position = new Vector3(startX, startY, 0f);
                currentPosition = 0f;
            }
        }
    }
}