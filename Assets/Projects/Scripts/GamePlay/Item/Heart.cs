using System.Collections.Generic;
using DG.Tweening;
using Projects.Scripts;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.SoundManager;
using UIController;
using UnityEngine;

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
                GameServiceManager.Instance.logEventManager.LogEvent("in_game_reward_click",new Dictionary<string, object>
                {
                    {"reward_for","heart"},
                    {"level","lv_"+GamePlayController.Instance.level}
                });
                GameServiceManager.Instance.adManager.ShowRewardedAd("in_game_heart_item", () =>
                {
                    LifeController.Instance.Addlife(GameDataManager.Instance.adLife);
                    GameServiceManager.Instance.logEventManager.LogEvent("in_game_reward_finish",new Dictionary<string, object>
                    {
                        {"reward_for","try_skin"},
                        {"level","lv_"+GamePlayController.Instance.level}
                    });
                    gameObject.SetActive(false);
                });
            }
            else
            {
                LifeController.Instance.Addlife(1);
                gameObject.SetActive(false);
            }

            audio.Play();
            gameObject.SetActive(false);
        }
    }
}