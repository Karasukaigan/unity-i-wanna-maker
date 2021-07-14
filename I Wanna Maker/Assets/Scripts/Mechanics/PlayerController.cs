using System;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        //Audio
        public AudioClip jumpAudio;
        public AudioClip doubleJumpAudio;
        public AudioClip shootAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        //Game Over IU
        public GameObject gameOverUI;
        public GameObject head;
        public GameObject[] blood;
        public float playerDeadTimer = 0f; //Used to realize the delayed display of the game end UI after the player dies
        private bool gameOverUIIsVisible = false;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 5f;

        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 15f;

        public float canJumpTime = 0.36f; //Maximum time you can jump
        public float JumpTimer = 0f; //Current jump time
        public int jumpMaxTimes = 2; //Maximum number of jumps
        public int jumpTimes = 2; //Current number of jumps
        //private bool canJump = true; //Can jump?

        public GameObject bullet;
        private Vector2 bulletSpeed = new Vector2(20,0);

        public JumpState jumpState = JumpState.InFlight;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool dead = false;
        public bool controlEnabled = true;

        bool jump;
        
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

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
                    jumpState = JumpState.PrepareToJump;
                    JumpTimer = 0f; //Jump time cleared
                    jumpTimes ++; //After pressing the jump button, the current number of jumps increases by 1
                    stopJump = false;
                }
                //Can jump a second time while in the air
                else if (jumpState == JumpState.InFlight && Input.GetButtonDown("Jump") && jumpTimes <= jumpMaxTimes - 1 && !onVine)
                {
                    jumpState = JumpState.PrepareToJump; //Change the jump state to PrepareToJump
                    JumpTimer = 0f;
                    jumpTimes ++;
                    stopJump = false;
                }
                //If the jump key is released during the jump up process, the up jump process will stop
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                //If the maximum jumpable time is exceeded, the jump process stops
                else if (JumpTimer > canJumpTime)
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                //If the maximum jumpable time is exceeded, the up-jumping process of the second-stage jump stops
                else if (jumpTimes == 2 && JumpTimer > canJumpTime - 0.1f)
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                if (Input.GetButtonDown("Jump") && inWater)
                {
                    jumpState = JumpState.PrepareToJump;
                    JumpTimer = 0f;
                    stopJump = false;
                    isVineJump = true;
                }

                //Shoot a bullet
                if (Input.GetButtonDown("FireIWanna"))
                {
                    if (shootAudio != null) audioSource.PlayOneShot(shootAudio);
                    //Generate bullets at the player's position
                    GameObject thisBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    //According to the player's orientation, give speed to the bullet
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
            
            //Determine whether the player is falling.
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
            //If on the vine
            if (onVine)
            {
                //Jump when leaving the vine
                if (velocity.x != 0 || Math.Abs(transform.position.x - vinePosition) > 0.2)
                {
                    onVine = false;
                    animator.SetBool("onVine", false);
                    if (Input.GetButton("Jump"))
                    {
                        jumpState = JumpState.PrepareToJump;
                        JumpTimer = 0f;
                        stopJump = false;
                        isVineJump = true;
                    }
                }
            }

            base.FixedUpdate();
        }

        //Update jump status
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
                    jumpTimes = 0; //Clear the number of jumps after landing
                    JumpTimer = 0f; //Jump time is cleared after landing
                    onVine = false; //Leave the vines after landing
                    animator.SetBool("onVine", false);
                    inWater = false; //Leave the water after landing
                    break;
            }
        }

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
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            //Used to flip left and right
            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            //Update animator variables
            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            //animator.SetFloat("velocityY", velocity.y);

            targetVelocity = move * maxSpeed;
        }

        //Enumerate jump state
        public enum JumpState
        {
            Grounded,      //Grounded=0
            PrepareToJump, //PrepareToJump=1
            Jumping,       //Jumping=2
            InFlight,      //InFlight=3
            Landed         //Landed =4
        }

        //Show the object
        public void Show()
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }

        //Hide the object
        public void Hide()
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        /// <summary>
        /// Teleport to some position.
        /// </summary>
        /// <param name="position"></param>
        public void Teleport(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        private void OnTriggerEnter2D(Collider2D collision) 
        {
            collision.SendMessage("IsPlayer", true, SendMessageOptions.DontRequireReceiver);
        }

    }
}