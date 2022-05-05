using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Item
{
    public class Spring : ObjectCollision
    {
        [SerializeField] private Vector2 force;
        [SerializeField] private Transform obj;
        [SerializeField] private new SimpleAudio audio;
       
        protected override async void CollisionEnter(string triggerTag, Transform triggerObject)
        {
            base.CollisionEnter(triggerTag, triggerObject);
            obj.DOKill(true);
            triggerObject.GetComponent<CharacterController>().SetVelocity(force);
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
            obj.DOPunchPosition(new Vector3(0, -0.36f, 0), 1f).SetEase(Ease.OutBounce);
            audio.Play();
            
        }
        
    }
}