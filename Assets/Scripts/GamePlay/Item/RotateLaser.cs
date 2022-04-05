using System;
using System.ComponentModel;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Item
{
    public class RotateLaser : MonoBehaviour
    {
        [SerializeField]
        private Transform container;
        [SerializeField] private bool clockWise;
        [SerializeField] private float speed;
        [SerializeField] private Laser[] lasers;
        [SerializeField,OnValueChanged(nameof(OnvalueChange))] private float maxRange = 5;
        private void Start()
        {
            foreach (var gun in lasers)
            {
                gun.SetRange(maxRange);
            }
            container.DORotate(new Vector3(0, 0, clockWise ? -360 : 360), speed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    foreach (var gun in lasers)
                    {
                        gun.AutoUpdateDirection();
                    }
                })
                .SetLoops(-1,LoopType.Incremental);
        }

        private void OnvalueChange()
        {
            foreach (var gun in lasers)
            {
                gun.SetRange(maxRange);
            }
        }
    }
}