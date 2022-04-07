using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Switches
{
    public class StandUpSwitch : BaseSwitch
    {
        
        [SerializeField] private Transform off,on;
        [SerializeField] private SimpleAudio simpleAudio;
        
       
        private int _countTrigger;
        private void Start()
        {
            off.gameObject.SetActive(false);
            on.gameObject.SetActive(false);
            switchObj.transform.position = !IsOn ? off.position : on.position;
        }

        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            _countTrigger++;
            if (triggerPlatform.CanSwitchByCinematic(true))
            {
                Debug.Log("can switch");
                GamePlayController.Instance.PauseForCinematic(true);
            }
            switchObj.transform.DOMove(on.position, actionDuration).SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal,true)
                .OnComplete(()=>SwitchOn(triggerObject));
            simpleAudio.Play();
        }

        protected override void TriggerExit(string triggerTag, Transform triggerObject)
        {
            base.TriggerExit(triggerTag, triggerObject);
            _countTrigger--;
            if (_countTrigger <= 0)
            {
                _countTrigger = 0;
                if (triggerPlatform.CanSwitchByCinematic(false))
                {
                    GamePlayController.Instance.PauseForCinematic(true);
                }
                switchObj.transform.DOMove(off.position, actionDuration) 
                    .SetUpdate(UpdateType.Normal,true)
                    .SetEase(Ease.Linear).OnComplete(()=>
                {
                    SwitchOff(triggerObject);
                    
                    simpleAudio.Play();
                });
            }
        }
    }
}