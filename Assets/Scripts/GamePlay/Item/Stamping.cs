using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Truongtv.SoundManager;
using Unity.Collections;
using UnityEngine;

namespace GamePlay.Item
{
    [ExecuteAlways]
    public class Stamping : MonoBehaviour
    {
        [SerializeField] private GameObject left, right;
        [SerializeField] private LineRenderer leftLine, rightLine;
        [SerializeField,OnValueChanged(nameof(OnvalueChange))] private float distance;
        [SerializeField] private float speed,delay,firstDelay;
        [SerializeField] private SimpleAudio audio;
        private float _delta = 0.85f;
        private void OnvalueChange()
        {
            left.transform.localPosition = new Vector3(-distance-_delta,0,0);
            right.transform.localPosition = new Vector3(distance+_delta,0,0);
            leftLine.positionCount = 2;
            leftLine.SetPositions(new[] {left.transform.position + left.transform.right*2f, left.transform.position});
            rightLine.positionCount = 2;
            rightLine.SetPositions(new[] {right.transform.position + right.transform.right*2f, right.transform.position});
        }

        private async void Start()
        {
            OnvalueChange();
            await UniTask.Delay(TimeSpan.FromSeconds(firstDelay));
            AutoMove();
        }

        private void AutoMove()
        {
            Move(delay,AutoReserveMove);
        }

        private void AutoReserveMove()
        {
            audio.Play();
            ReserveMove(delay,AutoMove);
        }

        private void Move(float delay,Action complete = null)
        {
            var time = distance / speed;
            left.transform
                .DOLocalMoveX(-_delta, time)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal,false)
                .SetDelay(delay)
                .OnUpdate(() =>
                {
                    leftLine.SetPosition(1,left.transform.position);
                })
                .OnComplete(() => { complete?.Invoke(); });
            right.transform
                .DOLocalMoveX(_delta, time)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal,false)
                .OnUpdate(() =>
                {
                    rightLine.SetPosition(1,right.transform.position);
                })
                .SetDelay(delay)
                .OnComplete(() => {});
        }
        private void ReserveMove(float delay,Action complete = null)
        {
            var time = distance / speed;
            left.transform
                .DOLocalMoveX(-distance-_delta, time)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnUpdate(() =>
                {
                    leftLine.SetPosition(1,left.transform.position);
                })
                .SetUpdate(UpdateType.Normal,false)
                .OnComplete(() => { complete?.Invoke(); });
            right.transform
                .DOLocalMoveX(distance+_delta, time)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnUpdate(() =>
                {
                    rightLine.SetPosition(1,right.transform.position);
                })
                .SetUpdate(UpdateType.Normal,false)
                .OnComplete(() => {  });
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
    }
}