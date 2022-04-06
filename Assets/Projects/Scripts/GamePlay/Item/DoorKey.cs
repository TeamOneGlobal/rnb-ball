using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Truongtv.SoundManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Item
{
    public class DoorKey : ObjectTrigger
    {
        [SerializeField,ValueDropdown(nameof(GetAllTriggerTag))] private string collisionTag;
        [SerializeField] private SimpleAudio simpleAudio;
        [SerializeField] private GameObject keyUi;
        [SerializeField] private GameObject effect;

        private void Start()
        {
            var position = transform.localPosition;
            var delay = Random.Range(0f, 0.5f);
            transform.DOLocalMoveY(position.y + 0.3f, 1f).SetDelay(delay).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        protected override void TriggerEnter(string triggerTag,Transform triggerObject)
        {
            if(!collisionTag.Equals(triggerTag)) return;
            OnTrigger().Forget();
        }

        private async UniTaskVoid OnTrigger()
        {
            KeyCollector.Instance.CollectKey(collisionTag,transform.position);
            Collider2D.enabled = false;
            keyUi.SetActive(false);
            effect.SetActive(true);
            simpleAudio.Play().Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            gameObject.SetActive(false);
            
        }
    }
}