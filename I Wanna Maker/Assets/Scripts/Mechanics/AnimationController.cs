using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// AnimationController集成了对物理和动画的控制，通常用于简单的敌人动画。未使用。
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : KinematicObject
    {
        /// <summary>
        /// 最大水平速度。
        /// </summary>
        [Tooltip("最大水平速度。")]
        public float maxSpeed = 5;
        /// <summary>
        /// 最大跳跃速度。
        /// </summary>
        [Tooltip("最大跳跃速度。")]
        public float jumpTakeOffSpeed = 7;

        /// <summary>
        /// 行进方向。
        /// </summary>
        [Tooltip("行进方向。")]
        public Vector2 move;

        /// <summary>
        /// 值为True时，跳跃。
        /// </summary>
        [Tooltip("跳跃。")]
        public bool jump;

        /// <summary>
        /// 值为True时，将当前跳跃速度设置为零。
        /// </summary>
        [Tooltip("是否停止跳跃。")]
        public bool stopJump;

        SpriteRenderer spriteRenderer;
        Animator animator;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }
    }
}