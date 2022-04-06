using System.Collections.Generic;
using DG.Tweening;
using GamePlay.CameraControl;
using GamePlay.Switches;
using UnityEngine;
using UnityEngine.Events;
using UpdateType = Com.LuisPedroFonseca.ProCamera2D.UpdateType;

namespace GamePlay.Platform
{
    public class TriggerPlatform : BaseMovingPlatform
    {
        public UnityEvent onTriggerEnd;
        [SerializeField] private List<BaseSwitch> switches;
        [SerializeField] private bool useCinematic;
        private bool _isOn;
        public bool CanSwitchByCinematic(bool newState)
        {
            if (!useCinematic) return false;
            if (newState) return !_isOn;
            var count = 0;
            foreach (var switchObj in switches)
            {
                if (switchObj.IsOn)
                {
                    count++;
                }
            }
            return count==1;
        }

        public void SwitchOn(Transform target)
        {
            if(_isOn) return;
            _isOn = true;
            platform.transform.DOKill();
            if (useCinematic)
            {
                useCinematic = false;
                CinematicView.Instance.StartCinematic(platform.transform, () =>
                {
                    PlaySoundMoving();
                    //CinematicView.Instance.ChangeTarget(platform.transform,UpdateType.FixedUpdate);
                    Move(true,() =>
                    {
                        
                        PlaySoundStop();
                        CinematicView.Instance.MoveBack(target, () =>
                        {
                            onTriggerEnd.Invoke();
                        });
                    }, () => { CinematicView.Instance.UpdateCameraPosition(platform.transform.position); });
                });
            }
            else
            {
                PlaySoundMoving();
                Move(true,PlaySoundStop);
            }
        }

        public void SwitchOff(Transform target)
        {
            if(!_isOn) return;
            foreach (var switchObj in switches)
            {
                if (switchObj.IsOn)
                {
                    return;
                }
            }
            _isOn = false;
            platform.transform.DOKill();
            if (useCinematic)
            {
                useCinematic = false;
                
                CinematicView.Instance.StartCinematic(platform.transform, () =>
                {
                    PlaySoundMoving();
                   
                    ReserveMove(true,() =>
                    {
                        PlaySoundStop();
                        CinematicView.Instance.MoveBack(target, () =>
                        {
                           // CinematicView.Instance.ChangeTarget(GamePlayController.Instance.controlCharacter.transform,UpdateType.FixedUpdate);
                        });
                    }, () => { CinematicView.Instance.UpdateCameraPosition(platform.transform.position); });
                });
            }
            else
            {
                PlaySoundMoving();
                ReserveMove(true,PlaySoundStop);
            }
        }
    }
}