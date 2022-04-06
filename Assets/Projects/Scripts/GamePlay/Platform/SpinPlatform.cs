using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Platform
{
    public class SpinPlatform : MonoBehaviour
    {
        [SerializeField,OnValueChanged(nameof(Awake))] private float range;
        [SerializeField] private bool clockWise;
        [SerializeField] private float speed;
        [SerializeField] private List<Transform> points;
        private void Awake()
        {
            foreach (var point in points)
            {
                var position = transform.position;
                var direction = (point.position - position).normalized;
                point.position = position + direction*range;
            }
            
        }

        private void Start()
        {
            transform.DORotate(new Vector3(0, 0, clockWise ? -180 : 180), speed).SetRelative(true)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }
    }
}