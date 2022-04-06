using GamePlay.Platform;
using UnityEngine;
using UnityEngine.Events;

namespace GamePlay.Switches
{
    public class BaseSwitch : ObjectTrigger
    {
        [SerializeField] protected TriggerPlatform triggerPlatform; 
        [SerializeField] protected Transform switchObj;
        [SerializeField] protected float actionDuration;
        [SerializeField] private UnityEvent switchOn, switchOff;
        private bool _isOn;
        public bool IsOn => _isOn;
        public void SwitchOn(Transform target)
        {
            _isOn = true;
            triggerPlatform?.SwitchOn(target);
            switchOn.Invoke();
        }

        public void SwitchOff(Transform target)
        {
            _isOn = false;
            triggerPlatform?.SwitchOff(target);
            switchOff.Invoke();
        }
    }
}