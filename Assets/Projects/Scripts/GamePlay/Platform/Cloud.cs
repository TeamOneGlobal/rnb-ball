using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace GamePlay.Platform
{
    public class Cloud : ObjectCollision
    {
        [SerializeField] private Collider2D collider2D;
        [SerializeField] private float existDuration,respawnDuration;
        [SerializeField] private SkeletonAnimation animation;
        [SerializeField, SpineAnimation] private string destroyAnim, respawnAnim;
        protected override async void CollisionEnter(string triggerTag, Transform triggerObject)
        {
            base.CollisionEnter(triggerTag, triggerObject);
            if(transform.position.y>=triggerObject.position.y) return;
            await UniTask.Delay(TimeSpan.FromSeconds(existDuration));
            animation.AnimationState.SetAnimation(0, destroyAnim, false);
            collider2D.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(respawnDuration));
            var trackEntry =  animation.AnimationState.SetAnimation(0, respawnAnim, false);
            trackEntry.Complete += entry => {collider2D.enabled = true; };
        }
    }
}