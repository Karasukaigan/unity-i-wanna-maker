using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 用来处理运动物体，为其实现物理效果，通常用于玩家。
    /// </summary>
    public class KinematicObject : MonoBehaviour
    {
        /// <summary>
        /// 允许实体站立的最小地面法线长度（点积）。
        /// </summary>
        public float minGroundNormalY = .65f;

        /// <summary>
        /// 应用于该实体的自定义重力系数。
        /// </summary>
        public float gravityModifier = 10f;

        /// <summary>
        /// 实体当前速度。
        /// </summary>
        public Vector2 velocity;

        /// <summary>
        /// 实体当前是否位于表面上？
        /// </summary>
        /// <value></value>
        public bool IsGrounded { get; private set; }

        protected Vector2 targetVelocity;
        protected Vector2 groundNormal;
        /// <summary>
        /// 该实体的2D刚体。
        /// </summary>
        protected Rigidbody2D body;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

        /// <summary>
        /// 最小移动距离。
        /// </summary>
        protected const float minMoveDistance = 0.001f;
        /// <summary>
        /// 外壳半径。
        /// </summary>
        protected const float shellRadius = 0.01f;

        /// <summary>
        /// 是否在藤蔓上。
        /// </summary>
        public bool onVine = false;
        /// <summary>
        /// 实体在藤蔓上时的坐标。
        /// </summary>
        public float vinePosition;
        /// <summary>
        /// 跳跃是否发生在藤蔓上。
        /// </summary>
        protected bool isVineJump = false;

        /// <summary>
        /// 是否在水中。
        /// </summary>
        public bool inWater = false;

        /// <summary>
        /// 以物体的垂直速度反弹。
        /// </summary>
        /// <param name="value"></param>
        public void Bounce(float value)
        {
            velocity.y = value;
        }

        /// <summary>
        /// 以指定速度反弹。
        /// </summary>
        /// <param name="dir"></param>
        public void Bounce(Vector2 dir)
        {
            velocity.y = dir.y;
            velocity.x = dir.x;
        }

        protected virtual void OnEnable()
        {
            body = GetComponent<Rigidbody2D>();
            body.isKinematic = true;
        }

        protected virtual void OnDisable()
        {
            body.isKinematic = false;
        }

        protected virtual void Start()
        {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        protected virtual void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity()
        {

        }

        protected virtual void FixedUpdate()
        {
            //如果已经下落
            if (velocity.y < 0)
            {
                //如果在藤蔓上或者在水中时，以固定速度缓慢下落
                if (onVine || inWater)
                {
                    velocity.y = -3f;
                }
                //以指定的重力系数下落
                else
                {
                    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
                }
            }
            //以重力系数减速上升
            else
                velocity += Physics2D.gravity * Time.deltaTime;

            velocity.x = targetVelocity.x;

            IsGrounded = false;

            var deltaPosition = velocity * Time.deltaTime;

            var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

            var move = moveAlongGround * deltaPosition.x;

            PerformMovement(move, false);

            move = Vector2.up * deltaPosition.y;

            PerformMovement(move, true);

        }

        void PerformMovement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                //检查是否在当前行进方向上碰到东西
                var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (var i = 0; i < count; i++)
                {
                    var currentNormal = hitBuffer[i].normal;

                    //判断表面是否足够平坦可以着陆
                    if (currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        //如果向上移动，则将groundNormal更改为新的表面法线
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }

                    if (IsGrounded)
                    {
                        //速度有多少与表面法线一致？
                        var projection = Vector2.Dot(velocity, currentNormal);
                        if (projection < 0)
                        {
                            //如果逆法线移动（上山），则速度较慢
                            velocity = velocity - projection * currentNormal;
                        }
                    }
                    //如果向上过程中碰到东西，则垂直方向速度归零
                    else if (yMovement)
                    {
                        velocity.y = Mathf.Min(velocity.y, 0);
                    }
                    else
                    {
                        //在空中撞到了什么东西，则归零水平速度。
                        velocity.x *= 0;
                    }
                    
                    //从实际移动距离中删除shellDistance。
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            body.position = body.position + move.normalized * distance;
        }
    }

}