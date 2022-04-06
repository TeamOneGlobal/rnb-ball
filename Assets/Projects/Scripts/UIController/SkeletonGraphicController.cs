using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace UIController
{
    [RequireComponent(typeof(SkeletonGraphic))]
    public class SkeletonGraphicController : MonoBehaviour
    {
        public SkeletonGraphic skeleton;
        private Action _callback;
        private void Awake()
        {
            skeleton = GetComponent<SkeletonGraphic>();
            skeleton.AnimationState.Complete += OnAnimationComplete;
            skeleton.AnimationState.Start += OnSpineAnimationStart;
        }

        public TrackEntry PlayAnim(string animName,int trackIndex,bool isLoop = false,Action complete = null)
        {
            var entry = skeleton.AnimationState.SetAnimation(trackIndex, animName,isLoop);
            entry.Complete += trackEntry => { complete?.Invoke();};
            return entry;
        }
        private void OnAnimationComplete(TrackEntry trackEntry)
        {
            _callback?.Invoke();

        }
        private void OnSpineAnimationStart(TrackEntry trackEntry)
        {
            _callback?.Invoke();

        }

        public void InitSkin(string skinName)
        {
            skeleton.initialSkinName = skinName;
            skeleton.Initialize(true);
        }
    }
}