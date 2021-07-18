using System;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 实现对玩家控制的类。它是AnimationController类的超集。
    /// </summary>
    public class PlayerController : KinematicObject
    {
        [Tooltip("一段跳音频，通常为sndJump01。")]
        public AudioClip jumpAudio;
        [Tooltip("二段跳音频，通常为sndJump02。")]
        public AudioClip doubleJumpAudio;
        [Tooltip("射击音频，通常为sndShoot。")]
        public AudioClip shootAudio;
        [Tooltip("复活音频，通常为sndNull。")]
        public AudioClip respawnAudio;
        [Tooltip("死亡音频，通常为sndDeathShort。")]
        public AudioClip ouchAudio;

        [Tooltip("GameOver的UI，通常为GameOver。")]
        public GameObject gameOverUI;
        [Tooltip("头，通常为Head。")]
        public GameObject head;
        [Tooltip("血滴，通常容量为3，Blood01~03。")]
        public GameObject[] blood;
        /// <summary>
        /// 计时器，用来实现玩家死亡后游戏结束UI的延迟显示。
        /// </summary>
        public float playerDeadTimer = 0f;
        private bool gameOverUIIsVisible = false;

        /// <summary>
        /// 玩家最大水平速度。
        /// </summary>
        [Tooltip("玩家最大水平速度。")]
        public float maxSpeed = 5f;
        /// <summary>
        /// 跳跃开始时的初始跳跃速度。
        /// </summary>
        [Tooltip("初始跳跃速度。")]
        public float jumpTakeOffSpeed = 15f;

        /// <summary>
        /// 最大跳跃时间。
        /// </summary>
        [Tooltip("最大跳跃时间。")]
        public float canJumpTime = 0.36f;
        /// <summary>
        /// 当前跳跃时间。
        /// </summary>
        [Tooltip("当前跳跃时间。")]
        public float JumpTimer = 0f;
        /// <summary>
        /// 最大跳跃次数。
        /// </summary>
        [Tooltip("最大跳跃次数。")]
        public int jumpMaxTimes = 2;
        /// <summary>
        /// 当前跳跃次数。
        /// </summary>
        [Tooltip("当前跳跃次数。")]
        public int jumpTimes = 2;

        /// <summary>
        /// 子弹的预制体。
        /// </summary>
        [Tooltip("子弹的预制体，通常为Bullet。")]
        public GameObject bullet;
        /// <summary>
        /// 子弹速度。
        /// </summary>
        private Vector2 bulletSpeed = new Vector2(20,0);

        /// <summary>
        /// 跳跃状态。
        /// </summary>
        public JumpState jumpState = JumpState.InFlight; //游戏开始时，玩家通常出生在空中，因此是InFlight
        /// <summary>
        /// 是否停止跳跃。
        /// </summary>
        private bool stopJump;

        public Collider2D collider2d;
        public AudioSource audioSource;
        public Health health;
        public bool dead = false;
        /// <summary>
        /// 是否允许操作。
        /// </summary>
        public bool controlEnabled = true;

        /// <summary>
        /// 是否在跳跃过程中。
        /// </summary>
        bool jump;
        
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        //public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                {
                    jumpState = JumpState.PrepareToJump; //将跳跃状态变为跳跃准备
                    JumpTimer = 0f; //跳跃时间清零
                    jumpTimes ++; //按下跳跃键后当前跳跃次数增加1
                    stopJump = false;
                }
                //在空中时可以跳第二次
                else if (jumpState == JumpState.InFlight && Input.GetButtonDown("Jump") && jumpTimes <= jumpMaxTimes - 1 && !onVine)
                {
                    jumpState = JumpState.PrepareToJump; //将跳跃状态变为跳跃准备
                    JumpTimer = 0f; //跳跃时间清零
                    jumpTimes ++; //按下跳跃键后当前跳跃次数增加1
                    stopJump = false;
                }
                //如果上跳过程中松开了跳跃键，则上跳过程停止
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                //如果超过了最大可跳跃时间，则上跳过程停止
                else if (JumpTimer > canJumpTime)
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                //如果超过了最大可跳跃时间，则二段跳的上跳过程停止
                else if (jumpTimes == 2 && JumpTimer > canJumpTime - 0.1f)
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                if (Input.GetButtonDown("Jump") && inWater)
                {
                    jumpState = JumpState.PrepareToJump; //将跳跃状态变为跳跃准备
                    JumpTimer = 0f; //跳跃时间清零
                    stopJump = false;
                    isVineJump = true;
                }

                //按下Shift键发射子弹时
                if (Input.GetButtonDown("FireIWanna"))
                {
                    if (shootAudio != null) audioSource.PlayOneShot(shootAudio);
                    //在玩家位置生成子弹
                    GameObject thisBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    //根据玩家朝向，给子弹赋予速度
                    thisBullet.GetComponent<Rigidbody2D>().velocity = spriteRenderer.flipX?-1*bulletSpeed:bulletSpeed;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
            if(JumpTimer < canJumpTime + 0.1) JumpTimer += Time.deltaTime; //增加当前跳跃时间
            
            //判断玩家是否正在下落
            if (velocity.y > 0)
            {
                animator.SetBool("falling", false);
            } 
            else 
            {
                animator.SetBool("falling", true);
            }

            if (playerDeadTimer > 0 && !gameOverUIIsVisible && dead)
            {
                playerDeadTimer -= Time.deltaTime;
            }

            if (playerDeadTimer <= 0 && !gameOverUIIsVisible && dead){
                gameOverUIIsVisible = true;
                GameObject.Instantiate(gameOverUI, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f), Quaternion.identity);
            }
        }

        protected override void FixedUpdate() {
            //如果玩家在藤蔓上，当玩家横向速度不再为0，或者X轴坐标移动超过0.2时，触发跳跃
            if (onVine)
            {
                if (velocity.x != 0 || Math.Abs(transform.position.x - vinePosition) > 0.2f)
                {
                    onVine = false;
                    animator.SetBool("onVine", false);
                    if (Input.GetButton("Jump"))
                    {
                        jumpState = JumpState.PrepareToJump; //将跳跃状态变为跳跃准备
                        JumpTimer = 0f; //跳跃时间清零
                        stopJump = false;
                        isVineJump = true;
                    }
                }
            }

            base.FixedUpdate();
        }

        /// <summary>
        /// 更新跳跃状态。
        /// </summary>
        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    jumpTimes = 0; //落地后跳跃次数清零
                    JumpTimer = 0f; //落地后跳跃时间清零
                    onVine = false; //落地后脱离藤蔓
                    animator.SetBool("onVine", false);
                    inWater = false; //落地后脱离水
                    break;
            }
        }

        /// <summary>
        /// 速度计算。
        /// </summary>
        protected override void ComputeVelocity()
        {
            //if (jump && IsGrounded)
            if (jump)
            {
                if (isVineJump)
                {
                    isVineJump = false;
                    velocity.y = (jumpTakeOffSpeed + 5f) * model.jumpModifier;
                    jump = false;
                }
                else
                {
                    velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                    jump = false;
                }
            }
            else if (stopJump)
            {
                stopJump = false;

                //如果stopJump为True时垂直方向依然有向上的速度，则减速
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            //左右翻转
            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            //更新动画器
            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            //animator.SetFloat("velocityY", velocity.y);

            targetVelocity = move * maxSpeed;
        }

        //枚举跳转状态
        public enum JumpState
        {
            Grounded,      //Grounded=0
            PrepareToJump, //PrepareToJump=1
            Jumping,       //Jumping=2
            InFlight,      //InFlight=3
            Landed         //Landed =4
        }

        /// <summary>
        /// 显示玩家。
        /// </summary>
        public void Show()
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }

        /// <summary>
        /// 隐藏玩家。
        /// </summary>
        public void Hide()
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        /// <summary>
        /// Teleport to some position.
        /// 传送到某个位置。
        /// </summary>
        /// <param name="targetPosition">传送的目标坐标。</param>
        public void Teleport(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        /// <summary>
        /// 当玩家和其他物体发生碰撞时，向与之发生碰撞的物体发送"IsPlayer"消息。必要时使用。
        /// </summary>
        /// <param name="collision">与之发生碰撞物体的碰撞器。</param>
        private void OnTriggerEnter2D(Collider2D collision) 
        {
            collision.SendMessage("IsPlayer", true, SendMessageOptions.DontRequireReceiver);
        }

    }
}