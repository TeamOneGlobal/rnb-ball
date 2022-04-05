using Spine.Unity;
using UnityEngine;

namespace UIController.SkinPopupController
{
    public class BallAnimationInSkinPopup : SkeletonGraphicController
    {
        [SerializeField, SpineAnimation] private string animJump,animIdle;

        public void Play()
        {
            PlayAnim(animJump,0,false, () =>
            {
                PlayAnim(animIdle, 0, true);
            });
        }

        public void PlayAnim(string anim, int trackIndex)
        {
            PlayAnim(anim,trackIndex,true);
        }
    }
}