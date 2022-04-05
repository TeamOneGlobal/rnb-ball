using System;
using UnityEngine;

namespace GamePlay.Item
{
    public class LightSource : MonoBehaviour
    {
        [SerializeField] private Light light;
        [SerializeField] private bool activeLight;

        private void Start()
        {
            ActiveLight(activeLight);
        }

        private void Update()
        {
            light.SetDirection(transform.up);
        }

        public void ActiveLight(bool active)
        {
            light.Active(active);
        }

    }
}