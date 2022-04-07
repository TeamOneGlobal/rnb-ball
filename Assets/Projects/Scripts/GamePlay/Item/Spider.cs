using System.Collections.Generic;
using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Item
{
    [ExecuteAlways]
    public class Spider : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private List<Transform> points;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float moveTime, delayTime;
        [SerializeField] private ChildrenTrigger trigger;
        [SerializeField] private CheckPoint hiddenCheckPoint;
        [SerializeField] private new SimpleAudio audio;
        [SerializeField] private Vector2 force;
        private void Start()
        {
            foreach (var point in points)
            {
                point.gameObject.SetActive(false);
            }
            target.transform.position = points[0].position;
            trigger.onTriggerEnter = (triggerTag, triggerObject) =>
            {
                triggerObject.GetComponent<CharacterController>().Damage(hiddenCheckPoint,force);
            };
            
            Init();
            MoveDown();
        }

        private void Init()
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0,points[0].position);
            lineRenderer.SetPosition(1, target.position);
        }

        private void MoveDown()
        {
            audio.Stop();
            target.DOMove(points[1].position, moveTime)
                .SetEase(Ease.Linear)
                .SetDelay(delayTime)
                .OnStart(()=>audio.Play())
                .OnUpdate(() => { lineRenderer.SetPosition(1, target.position); })
                .OnComplete(MoveUp);
        }

        private void MoveUp()
        {
            audio.Stop();
            target.DOMove(points[0].position, moveTime)
                .SetEase(Ease.Linear)
                .SetDelay(delayTime)
                .OnStart(()=>audio.Play())
                .OnUpdate(() => { lineRenderer.SetPosition(1, target.position); })
                .OnComplete(MoveDown);
        }
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                target.transform.position = points[1].position;
                Init();
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
        
    }
}