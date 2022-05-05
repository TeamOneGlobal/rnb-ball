using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Item
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation anim;
        [SerializeField, SpineAnimation] private string idle, move;
        [SerializeField] private bool flip;
        [SerializeField] private Transform target;
        [SerializeField] private List<Transform> points;
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
            
            MoveDown();
        }

        

        private void MoveDown()
        {
            anim.state.SetAnimation(0, idle, true);
            audio.Stop();
            
            target.DOMove(points[1].position, moveTime)
                .SetEase(Ease.Linear)
                .SetDelay(delayTime)
                .OnStart(()=>
                {
                    if (points[1].position.x > points[0].position.x)
                    {
                        target.transform.localScale = new Vector3(1,1,1);
                    }
                    else
                    {
                        target.transform.localScale = new Vector3(-1,1,1);
                    }
                    anim.state.SetAnimation(0, move, true);
                    audio.Play();
                })
                .OnComplete(MoveUp);
        }

        private void MoveUp()
        {
            anim.state.SetAnimation(0, idle, true);
            audio.Stop();
            
            target.DOMove(points[0].position, moveTime)
                .SetEase(Ease.Linear)
                .SetDelay(delayTime)
                .OnStart(()=>
                {
                    if (points[1].position.x > points[0].position.x)
                    {
                        target.transform.localScale = new Vector3(-1,1,1);
                    }
                    else
                    {
                        target.transform.localScale = new Vector3(1,1,1);
                    }
                    anim.state.SetAnimation(0, move, true);
                    audio.Play();
                })
                .OnComplete(MoveDown);
        }
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                target.transform.position = points[1].position;
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
    }
}