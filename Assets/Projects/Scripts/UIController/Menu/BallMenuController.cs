using Spine.Unity;
using UnityEngine;

namespace Projects.Scripts.UIController.Menu
{
    public class BallMenuController : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic ball;
        [SerializeField, SpineAnimation] private string[] mix;
        [SerializeField, SpineAnimation] private string  loseAnim;

        public void ApplySkin(string skin)
        {
            ball.initialSkinName = skin;
            ball.Initialize(true);
        }

        public void PlayRandomMix()
        {
            var r = Random.Range(0, mix.Length);
            var entry = ball.AnimationState.SetAnimation(0, mix[r], false);
            entry.Complete += trackEntry =>
            {
                PlayRandomMix();
            };
        }

        public void PlayLose()
        {
            ball.AnimationState.SetAnimation(0, loseAnim, true);
        }
    }
}