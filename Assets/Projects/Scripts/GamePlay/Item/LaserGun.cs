using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Item
{
    public class LaserGun : MonoBehaviour
    {
        [SerializeField] private Laser laser;
        [SerializeField] private Transform gun;
        [SerializeField] private bool autoMoving;
        [SerializeField,OnValueChanged(nameof(OnAngleChange))] private float startAngle;
        [SerializeField,ShowIf(nameof(autoMoving))] private float rotateTime,delayTime, endAngle;
        [SerializeField] private float maxRange;
        private void OnAngleChange()
        {
            if(gun!=null)
                gun.eulerAngles = new Vector3(0,0,startAngle+90);
            UpdateDirection();
        }

        private void Start()
        {
            OnAngleChange();
            SetRange(maxRange);
            if (autoMoving)
            {
                Move();
            }
        }

        public void SetRange(float range)
        {
            laser.SetRange(range);
        }
        public void UpdateDirection()
        {
            var direction = gun.transform.right;
            laser.SetDirection(direction);
        }

        private void Move()
        {
            gun.DORotate(new Vector3(0, 0, endAngle + 90), rotateTime)
                .SetEase(Ease.Linear)
                .SetDelay(delayTime)
                .SetRelative(false)
                .OnUpdate(UpdateDirection)
                .OnComplete(ReserveMove);
        }

        private void ReserveMove()
        {
            gun.DORotate(new Vector3(0, 0, startAngle + 90), rotateTime)
                .SetEase(Ease.Linear)
                .SetDelay(delayTime)
                .SetRelative(false)
                .OnUpdate(UpdateDirection)
                .OnComplete(Move);
        }
    }
}