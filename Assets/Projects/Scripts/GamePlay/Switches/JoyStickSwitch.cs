using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Switches
{
    public class JoyStickSwitch : BaseSwitch
    {
        [SerializeField] private float offAngle, onAngle;
        [SerializeField] private bool oneTime;
        [SerializeField] private SimpleAudio simpleAudio;
        private bool _trigger;
        private void Start()
        {
            var angle = !IsOn ? offAngle : onAngle;
            switchObj.transform.localEulerAngles = oneTime ? new Vector3(0,0,0) : new Vector3(0,0,angle);
        }
        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            if(oneTime && _trigger) return;
            _trigger = true;
            base.TriggerEnter(triggerTag, triggerObject);
            
            

            if (!oneTime)
            {
                if (!IsOn)
                {
                    if(onAngle * transform.lossyScale.x>0 && triggerObject.position.x< transform.position.x) return;    
                    if(onAngle * transform.lossyScale.x<0 && triggerObject.position.x> transform.position.x) return;    
                }
                else
                {
                    if(offAngle * transform.lossyScale.x>0 && triggerObject.position.x< transform.position.x) return;    
                    if(offAngle * transform.lossyScale.x<0 && triggerObject.position.x> transform.position.x) return;    
                }
            }
            if (triggerPlatform!=null&&triggerPlatform.CanSwitchByCinematic(!IsOn))
            {
                GamePlayController.Instance.PauseForCinematic(true);
            }
            simpleAudio.Play();
            var angle = !IsOn ?  onAngle:offAngle;
            if (oneTime)
                angle = 90;
            if (triggerObject.position.x < transform.position.x && oneTime)
            {
                angle *= -1;
            }
            switchObj.transform.DOLocalRotate(new Vector3(0,0,angle), actionDuration)
                .SetUpdate(UpdateType.Normal,true)
                .SetEase(Ease.Linear)
                .OnComplete(
                () =>
                {
                    if (!IsOn)
                        SwitchOn(triggerObject);
                    else
                        SwitchOff(triggerObject);
                });
        }
    }
}