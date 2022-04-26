using System;
using System.Collections;
using DG.Tweening;
using Spine;
using Spine.Unity;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Door
{
    public class BasicGate : ObjectTrigger
    {
        [SerializeField] private SkeletonAnimation gatenAnim;
        [SerializeField,SpineAnimation] private string idleAnim, openAnim, openedIdleAnim;
        [SerializeField] private SimpleAudio simpleAudio;
        [SerializeField] private AudioClip open, close,setKey;
    
        private bool _isPlayKey,_isOpen;

        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            if (triggerTag.Equals(collisionTags[0]) && KeyCollector.Instance.IsKeyCollected(collisionTags[0]))
            {
                OpenGate();
                triggerObject.GetComponent<CharacterController>().PlayCallAnim();
                OntriggerPlaySound();
            }
        }
        protected override void TriggerExit(string triggerTag, Transform triggerObject)
        {
            if (triggerTag.Equals(collisionTags[0]) && KeyCollector.Instance.IsKeyCollected(collisionTags[0]))
            {
                CloseGate();
                triggerObject.GetComponent<CharacterController>().PlayIdle();
                simpleAudio.Stop();
                simpleAudio.Play(close, delay: 0.3f);
            }
        }
        private void  OntriggerPlaySound()
        {
            if (!_isPlayKey)
            {
                _isPlayKey = true;

                IEnumerator playSound()
                {
                    simpleAudio.Play(setKey);
                    yield return new WaitForSeconds(0.3f);
                    simpleAudio.Play(open);
                }

                StartCoroutine(playSound());
            }
            else
            {
                simpleAudio.Stop();
                simpleAudio.Play(open);
            }
        }
        protected void OpenGate(Action onStart = null, Action onComplete = null)
        {
            if(_isOpen) return;
            _isOpen = true;
            var entry = gatenAnim.state.SetAnimation(0, openAnim, false);
            entry.Complete+= delegate(TrackEntry trackEntry)
            {
                gatenAnim.state.SetAnimation(0, openedIdleAnim, true);
            };
            GamePlayController.Instance.OpenGate(collisionTags[0]);
        }
        

        protected void CloseGate(Action onStart = null, Action onComplete = null)
        {
             GamePlayController.Instance.CloseGate(collisionTags[0]);
        }
    }
}