using System;
using System.Collections.Generic;
using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Platform
{
    [ExecuteAlways]
    public class BaseMovingPlatform : MonoBehaviour
    {
        [SerializeField] protected Platform platform;
        [SerializeField] protected Transform pointParent;
        [SerializeField] protected LineRenderer lineRenderer;
        [SerializeField] protected float speed;
        [SerializeField] protected List<Transform> points;
        [SerializeField] private SimpleAudio simpleAudio;
        [SerializeField] private AudioClip moving, stop;
        protected int CurrentPoint;

        
        private void Awake()
        {
            Init();
        }

        protected Transform GetTarget()
        {
            return points[0];
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
            platform.transform
                .DOMove(points[CurrentPoint + 1].position, time)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal,ignoreTimeScale)
                .OnUpdate(()=>onUpdate?.Invoke())
                .OnComplete(() => { Move(ignoreTimeScale,onComplete); });
            CurrentPoint++;

        }

        public void PlaySoundMoving()
        {
            simpleAudio.Play(moving, true);
        }
        public void PlaySoundStop()
        {
            simpleAudio.Play(stop);
        }

        protected void ReserveMove(bool ignoreTimeScale = false,Action onComplete = null,Action onUpdate = null)
        {

            if (CurrentPoint == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var distance = Vector2.Distance(points[CurrentPoint].position, points[CurrentPoint - 1].position);
            var time = distance / speed;
            platform.transform
                .DOMove(points[CurrentPoint - 1].position, time)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal,ignoreTimeScale)
                .OnUpdate(()=>onUpdate?.Invoke())
                .OnComplete(() => { ReserveMove(ignoreTimeScale,onComplete); });
            CurrentPoint--;
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
            platform.transform.position = points[0].position;
            lineRenderer.positionCount = positionList.Count;
            lineRenderer.SetPositions(positionList.ToArray());
            lineRenderer.useWorldSpace = true;
        }
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
    }
}