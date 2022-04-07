using System;
using System.Collections;
using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Door
{
    public class BasicGate : ObjectTrigger
    {
        [SerializeField] private SimpleAudio simpleAudio;
        [SerializeField] private AudioClip open, close,setKey;
        [SerializeField] private Transform gate;
        [SerializeField] private float openY, closeY=-4f;

        private bool _isPlayKey;
        void Start()
        {
            gate.transform.localPosition = new Vector3(0,closeY,0);
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
            gate.DOLocalMoveY(openY, 1f)
                .SetEase(Ease.Linear)
                .OnStart(() => { onStart?.Invoke();})
                .OnComplete(() => { onComplete?.Invoke();});
            GamePlayController.Instance.OpenGate(collisionTags[0]);
        }
        

        protected void CloseGate(Action onStart = null, Action onComplete = null)
        {
            gate.DOLocalMoveY(closeY, 1f)
                .SetEase(Ease.OutBounce)
                .OnStart(() => { onStart?.Invoke();})
                .OnComplete(() => { onComplete?.Invoke();});
             GamePlayController.Instance.CloseGate(collisionTags[0]);
        }
    }
}