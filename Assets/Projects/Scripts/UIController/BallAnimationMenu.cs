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
            PlayRandomMix(bodies, 0);
            PlayRandomMix(eyes, 1);
            PlayRandomMix(mouths, 2);
        }

        private void PlayRandomMix(string[] anims,int index)
        {
            var r = Random.Range(0, anims.Length);
            PlayAnim(anims[r], index, false, () => { PlayRandomMix(anims,index); });
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