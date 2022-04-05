using DG.Tweening;
using Truongtv.SoundManager;
using UIController;
using UnityEngine;
using UserDataModel;

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
                NetWorkHelper.ShowRewardedAdInGame("Rewarded_MagnetInGame", adResult:result =>
                {
                    if (!result) return;
                    UserDataController.UpdateMagnetDuration(Config.REWARDED_MAGNET_DURATION);
                    MagneticController.Instance.ActiveMagnetic();
                    
                    gameObject.SetActive(false);
                });
                
                
            }
            else
            {
                UserDataController.UpdateMagnetDuration(Config.FREE_MAGNET_DURATION);
                MagneticController.Instance.ActiveMagnetic();
                gameObject.SetActive(false);
            }
            audio.Play().Forget();
            
        }
    }
}