using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Events;

namespace GamePlay.Item
{
    public class LightReceiver : MonoBehaviour
    {
        public UnityEvent OnLightReceive, OnLightInactive;
        private bool isReceiveLight;
        public void LightTrigger()
        {
            if (!isReceiveLight) 
                OnLightReceive?.Invoke();
            isReceiveLight = true;
            CancelInvoke();
            Invoke(nameof(LightInActive),0.5f);
        }

        private void LightInActive()
        {
            isReceiveLight = false;
            OnLightInactive.Invoke();
        }
    }
}