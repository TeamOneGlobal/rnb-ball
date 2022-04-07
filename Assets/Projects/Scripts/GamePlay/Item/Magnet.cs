using System.Collections.Generic;
using DG.Tweening;
using Projects.Scripts;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.SoundManager;
using UIController;
using UnityEngine;

namespace GamePlay.Item
{
    public class Magnet : ObjectTrigger
    {
        [SerializeField] private bool adsItem;
        [SerializeField] private new SimpleAudio audio;
        private void Start()
        {
            var position = transform.localPosition;
            transform.DOLocalMoveY(position.y + 0.3f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            if (adsItem)
            {
                GamePlayController.Instance.controlCharacter.CancelAllMove();
                GameServiceManager.Instance.logEventManager.LogEvent("in_game_reward_click",new Dictionary<string, object>
                {
                    {"reward_for","magnet"},
                    {"level","lv_"+GamePlayController.Instance.level}
                });
                GameServiceManager.Instance.adManager.ShowRewardedAd("in_game_magnet_item", () =>
                {
                    GameDataManager.Instance.IncreaseMagnetDuration(GameDataManager.Instance.magnetDuration);
                    MagneticController.Instance.ActiveMagnetic();
                    GameServiceManager.Instance.logEventManager.LogEvent("in_game_reward_finish",new Dictionary<string, object>
                    {
                        {"reward_for","magnet"},
                        {"level","lv_"+GamePlayController.Instance.level}
                    });
                    gameObject.SetActive(false);
                });
            }
            else
            {
                GameDataManager.Instance.IncreaseMagnetDuration(GameDataManager.Instance.magnetDuration);
                MagneticController.Instance.ActiveMagnetic();
                gameObject.SetActive(false);
            }
            audio.Play();
            
        }
    }
}