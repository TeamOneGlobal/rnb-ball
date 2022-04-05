using Spine.Unity;
using UnityEngine;

namespace UIController
{
    public class BallAnimationMenu : SkeletonGraphicController
    {
        [SerializeField, SpineAnimation] private string[] winAnims;
        [SerializeField, SpineAnimation] private string[] bodies, eyes, mouths;


        public void PlayRandomState()
        {
            var r = Random.Range(0, bodies.Length);
            PlayAnim(bodies[r], 0, true);
            r = Random.Range(0, eyes.Length);
            PlayAnim(eyes[r], 1, true);
            r = Random.Range(0, mouths.Length);
            PlayAnim(mouths[r], 2, true);
        }

        public void PlayWin()
        {
            for (var i = 0; i < winAnims.Length; i++)
            {
                PlayAnim(winAnims[i], i, true);
            }
        }

        public void ApplySkin(string skin)
        {
            skeleton.initialSkinName = skin;
            skeleton.Initialize(true);
        }
    }
}