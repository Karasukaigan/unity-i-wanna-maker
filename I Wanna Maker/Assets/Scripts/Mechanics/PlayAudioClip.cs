using UnityEngine;

/// <summary>
/// 此类允许在动画期间播放音频。
/// 这个类来自平台游戏Microgame模板。未使用。
/// </summary>
public class PlayAudioClip : StateMachineBehaviour
{
    /// <summary>
    /// 剪辑应播放的标准化时间点。
    /// </summary>
    public float t = 0.5f;
    /// <summary>
    /// 如果大于零，则归一化时间将为（归一化时间%模数）。这用于在动画循环时重复音频。
    /// </summary>
    public float modulus = 0f;

    /// <summary>
    /// 要播放的音频。
    /// </summary>
    public AudioClip clip;
    float last_t = -1f;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var nt = stateInfo.normalizedTime;
        if (modulus > 0f) nt %= modulus;
        if (nt >= t && last_t < t)
            AudioSource.PlayClipAtPoint(clip, animator.transform.position);
        last_t = nt;
    }
}
