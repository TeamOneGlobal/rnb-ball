using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Platform
{
    public class Platform : ObjectCollision
    {
        [SerializeField] private bool fixedRotation = true;

        [SerializeField, ShowIf(nameof(fixedRotation))]
        private Vector3 fixedAngle;
        private int _countPlayer;

        public Action onCharacterEnter, onCharacterExit;
        
        private void Start()
        {
            _countPlayer = 0;
        }

        private void Update()
        {
            if(!fixedRotation) return;
            transform.eulerAngles = fixedAngle;
        }

        protected override void CollisionEnter(string triggerTag, Transform triggerObject)
        {
            base.CollisionEnter(triggerTag, triggerObject);
            if(triggerObject.position.y<=transform.position.y+0.5f) return;
            _countPlayer++;
            triggerObject.SetParent(transform);
            
            onCharacterEnter?.Invoke();
        }

        protected override void CollisionExit(string triggerTag, Transform triggerObject)
        {
            base.CollisionExit(triggerTag, triggerObject);
            _countPlayer--;
            triggerObject.SetParent(null);
            triggerObject.transform.localScale = new Vector3(1,1,1);
            if (_countPlayer <= 0)
            {
                _countPlayer = 0;
                onCharacterExit?.Invoke();
            }
        }
    }
}