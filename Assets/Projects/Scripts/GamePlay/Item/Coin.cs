using DG.Tweening;
using GamePlay.Characters;
using Truongtv.SoundManager;
using UnityEngine;
using UserDataModel;

namespace GamePlay.Item
{
    public class Coin : ObjectTrigger
    {
        [SerializeField] private GameObject effect;
        [SerializeField] private SimpleAudio simpleAudio;
        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            if (TagManager.PlayerTag.Contains(triggerTag)) //collision with characters, disappear
            {
                OnCollect();
                return;
            }

            //collision with characters,move to character then disappear
           
            if (!TagManager.MAGNET_TAG.Equals(triggerTag)) return;
            Collider2D.enabled = false;
            var duration = 0.5f;
            var tween = transform.parent.DOMove(triggerObject.position, duration);
            triggerObject.GetComponent<MagneticEffect>().SetCollectCoin();
            tween.OnUpdate(() =>
            {
                duration -= tween.Elapsed();
                if(duration>0)
                    tween.ChangeEndValue(triggerObject.position,duration,true);
               
            });
            tween.OnComplete(()=>
            {
                triggerObject.GetComponent<MagneticEffect>().SetCollectComplete();
                OnCollect();
            });
            
        }

        private void OnCollect()
        {
            
            CoinCollector.Instance.Collect(UserDataController.GetCurrentLevel()>GamePlayController.Instance.level
                ? 1
                : Config.COIN_VALUE_IN_GAME,transform.position);

            simpleAudio.Play().Forget();
            effect.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}