using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Projects.Scripts;
using Sirenix.OdinInspector;
using Spine.Unity;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Item
{
    public class TrySkin : ObjectTrigger
    {
        [SerializeField,OnValueChanged(nameof(OnValueChange)),ValueDropdown(nameof(GetAllSkinName))] private string trySkinName;
        [SerializeField] private new SimpleAudio audio;
        [SerializeField] private SkeletonAnimation skeletonRed,skeletonBlue;
        [SerializeField] private bool adsItem = true;
        private void Start()
        {
            var position = transform.localPosition;
            transform.DOLocalMoveY(position.y + 0.3f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            var totalUnlock = GameDataManager.Instance.GetUnlockedSkin();
            var data = GetAllSkinName().Except(totalUnlock).ToList();
            if (data.Count == 0)
            {
                data = GetAllSkinName();
            }
            trySkinName = data[Random.Range(0, data.Count)];
            OnValueChange();
        }

        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            if (adsItem)
            {
                GamePlayController.Instance.controlCharacter.CancelAllMove();
                GameServiceManager.Instance.logEventManager.LogEvent("in_game_reward_click",new Dictionary<string, object>
                {
                    {"reward_for","try_skin"},
                    {"level","lv_"+GamePlayController.Instance.level}
                });
                GameServiceManager.Instance.adManager.ShowRewardedAd("in_game_try_skin_item", () =>
                {
                    ActiveSkin();
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
                ActiveSkin();
                gameObject.SetActive(false);
            }

            audio.Play();
            
        }

        private void OnValueChange()
        {
            skeletonRed.initialSkinName = trySkinName + "_1";
            skeletonBlue.initialSkinName = trySkinName + "_2";
            skeletonRed.Initialize(true);
            skeletonBlue.Initialize(true);
        }
        private List<string> GetAllSkinName()
        {
            var result = new List<string>();
            var totalSkin = skeletonRed.SkeletonDataAsset.GetSkeletonData(true).Skins.Items;
            foreach (var skin in totalSkin)
            {
                result.Add(skin.Name.Split('_')[0]);
            }

            result.Remove("default");
            result.Remove("0");
            return result;
        }

        private void ActiveSkin()
        {
            GamePlayController.Instance.red.TrySkin(trySkinName);
            GamePlayController.Instance.blue.TrySkin(trySkinName);
        }
    }
}