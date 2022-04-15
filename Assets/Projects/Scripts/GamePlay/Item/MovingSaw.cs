using System;
using System.Collections.Generic;
using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Item
{
    [ExecuteAlways]
    public class MovingSaw : MonoBehaviour
    {
        [SerializeField] protected Transform saw;
        [SerializeField] private float sawRadius = 0.8f;
        [SerializeField] protected Transform pointParent;
        [SerializeField] protected LineRenderer lineRenderer;
        [SerializeField] protected float speed;
        [SerializeField] protected List<Transform> points;
       
        protected int CurrentPoint;
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Init();
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
        private void Init()
        {
            CurrentPoint = 0;
            points = new List<Transform>();
            var positionList = new List<Vector3>();
            foreach (Transform child in pointParent)
            {
                points.Add(child);
                positionList.Add(child.position);
            }

            if (points.Count <= 1) return;
            var direction = (points[1].position - points[0].position).normalized;
            saw.position = points[0].position+direction*sawRadius;
            lineRenderer.positionCount = positionList.Count;
            lineRenderer.SetPositions(positionList.ToArray());
            lineRenderer.useWorldSpace = true;
        }
        private void Start()
        {
            CurrentPoint = 0;
            PlaySoundMoving();
            AutoMove();
        }

        private void AutoMove()
        {
            Move(false,AutoReserveMove);
        }

        private void AutoReserveMove()
        {
            ReserveMove(false,AutoMove);
        }
        protected void Move(bool ignoreTimeScale = false,Action onComplete = null,Action onUpdate = null)
        {
            if (CurrentPoint >= points.Count - 1)
            {
                onComplete?.Invoke();
                return;
            }

            var distance = Vector2.Distance(points[CurrentPoint].position, points[CurrentPoint + 1].position);
            var time = distance / speed;
            var direction = (points[CurrentPoint + 1].position - points[CurrentPoint].position).normalized;
            saw.transform
                .DOMove(points[CurrentPoint + 1].position-direction*sawRadius, time)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal,ignoreTimeScale)
                .OnUpdate(()=>onUpdate?.Invoke())
                .OnComplete(() => { Move(ignoreTimeScale,onComplete); });
            CurrentPoint++;

        }

        public void PlaySoundMoving()
        {
            //simpleAudio.Play(moving, true);
        }
        public void PlaySoundStop()
        {
            //simpleAudio.Play(stop);
        }

        protected void ReserveMove(bool ignoreTimeScale = false,Action onComplete = null,Action onUpdate = null)
        {

            if (CurrentPoint == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var distance = Vector2.Distance(points[CurrentPoint].position, points[CurrentPoint - 1].position);
            var direction = (points[CurrentPoint -1].position - points[CurrentPoint].position).normalized;
            var time = distance / speed;
            saw.transform
                .DOMove(points[CurrentPoint - 1].position-direction*sawRadius, time)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal,ignoreTimeScale)
                .OnUpdate(()=>onUpdate?.Invoke())
                .OnComplete(() => { ReserveMove(ignoreTimeScale,onComplete); });
            CurrentPoint--;
        }
    }
}