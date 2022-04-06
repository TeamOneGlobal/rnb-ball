using System;
using DG.Tweening;
using Truongtv.SoundManager;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Door
{
    public class GateFactory : ObjectTrigger
    {
        [SerializeField] private SimpleAudio simpleAudio;
        [SerializeField] private AudioClip open, close,setKey;
        [SerializeField] private Transform gateLeft,gateRight;
        [SerializeField] private float openX, closeX;
        private bool _isPlayKey;
        void Start()
        {
           
        }

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
                simpleAudio.Play(close, delay: 0.3f).Forget();
            }
        }
        private void  OntriggerPlaySound()
        {
            if (!_isPlayKey)
            {
                simpleAudio.Play(setKey,onComplete: () => { simpleAudio.Play(open).Forget(); }).Forget();
                _isPlayKey = true;
            }
            else
            {
                simpleAudio.Stop();
                simpleAudio.Play(open).Forget();
            }
        }
        protected void OpenGate(Action onStart = null, Action onComplete = null)
        {
            gateLeft.DOLocalMoveX(-openX, 1f)
                .SetEase(Ease.Linear)
                .OnStart(() => { onStart?.Invoke();});
            gateRight.DOLocalMoveX(openX, 1f)
                .SetEase(Ease.Linear)
                .OnStart(() => { onStart?.Invoke();})
                .OnComplete(() => { onComplete?.Invoke();});
            GamePlayController.Instance.OpenGate(collisionTags[0]);
        }
        

        protected void CloseGate(Action onStart = null, Action onComplete = null)
        {
            gateLeft.DOLocalMoveX(-closeX, 1f)
                .SetEase(Ease.Linear)
                .OnStart(() => { onStart?.Invoke();});
            gateRight.DOLocalMoveX(closeX, 1f)
                .SetEase(Ease.Linear)
                .OnStart(() => { onStart?.Invoke();})
                .OnComplete(() => { onComplete?.Invoke();});
            GamePlayController.Instance.CloseGate(collisionTags[0]);
        }
    }
}