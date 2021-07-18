using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;


namespace Platformer.Mechanics
{
    /// <summary>
    /// 此类包含实现金币收集机制所需的数据。
    /// 这个类来自平台游戏Microgame模板。未使用。
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
        public AudioClip tokenCollectAudio;
        [Tooltip("如果为true，动画将从序列中的随机位置开始。")]
        public bool randomAnimationStartTime = false;
        [Tooltip("构成动画帧列表。")]
        public Sprite[] idleAnimation, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];

        internal SpriteRenderer _renderer;

        //由TokenController在场景中分配的唯一索引。
        internal int tokenIndex = -1;
        internal TokenController controller;
        //动画中的活动帧，由控制器更新。
        internal int frame = 0;
        internal bool collected = false;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);
            sprites = idleAnimation;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //仅当玩家与此金币发生碰撞时才执行OnPlayerEnter。
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

        void OnPlayerEnter(PlayerController player)
        {
            if (collected) return;
            //禁用游戏对象并将其从控制器更新列表中删除。
            frame = 0;
            sprites = collectedAnimation;
            if (controller != null)
                collected = true;
            var ev = Schedule<PlayerTokenCollision>();
            ev.token = this;
            ev.player = player;
        }
    }
}