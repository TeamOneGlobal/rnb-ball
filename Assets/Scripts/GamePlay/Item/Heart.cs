using DG.Tweening;
using ThirdParties.Truongtv;
using Truongtv.Services.Ad;
using Truongtv.SoundManager;
using UIController;
using UnityEngine;
using UserDataModel;

namespace GamePlay.Item
{
    public class Heart : ObjectTrigger
    {
        [SerializeField] private bool adItem = true;
        [SerializeField] private new SimpleAudio audio;
        private void Start()
        {
            var position = transform.localPosition;
            transform.DOLocalMoveY(position.y + 0.3f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            GamePlayController.Instance.controlCharacter.CancelAllMove();
            if (adItem)
            {
                GameServiceManager.Instance.adManager.ShowRewardedAd("in_game_heart_item", () =>
                {
                    LifeController.Instance.Addlife(Config.REWARDED_FREE_LIFE);
                    gameObject.SetActive(false);
                });
            }
            else
            {
                LifeController.Instance.Addlife(1);
                gameObject.SetActive(false);
            }

            audio.Play().Forget();
            gameObject.SetActive(false);
        }
    }
}