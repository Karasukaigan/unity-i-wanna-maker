using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 为场景中所有的金币设置动画。
    /// 这个类来自平台游戏Microgame模板。未使用。
    /// </summary>
    public class TokenController : MonoBehaviour
    {
        [Tooltip("动画帧率。")]
        private float frameRate = 6;
        [Tooltip("金币实例.")]
        public TokenInstance[] tokens;

        float nextFrameTime = 0;

        [ContextMenu("查找所有金币")]
        void FindAllTokensInScene()
        {
            tokens = UnityEngine.Object.FindObjectsOfType<TokenInstance>();
        }

        void Awake()
        {
            if (tokens.Length == 0)
                FindAllTokensInScene();
            for (var i = 0; i < tokens.Length; i++)
            {
                tokens[i].tokenIndex = i;
                tokens[i].controller = this;
            }
        }

        void Update()
        {
            if (Time.time - nextFrameTime > (1f / frameRate))
            {
                for (var i = 0; i < tokens.Length; i++)
                {
                    var token = tokens[i];
                    if (token != null)
                    {
                        token._renderer.sprite = token.sprites[token.frame];
                        if (token.collected && token.frame == token.sprites.Length - 1)
                        {
                            token.gameObject.SetActive(false);
                            tokens[i] = null;
                        }
                        else
                        {
                            token.frame = (token.frame + 1) % token.sprites.Length;
                        }
                    }
                }
                nextFrameTime += 1f / frameRate;
            }
        }

    }
}