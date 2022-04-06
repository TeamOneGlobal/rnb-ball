using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace GamePlay.Item
{
    public class Rope : ObjectTrigger
    {
        [SerializeField] private SkeletonAnimation anim;
        [SerializeField] private string contact;

        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            anim.AnimationState.SetAnimation(0, contact, false);
        }
    }
}