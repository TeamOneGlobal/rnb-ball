using System;
using DG.Tweening;
using GamePlay;
using Projects.Scripts.UIController;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace Projects.Scripts.GamePlay.Platform
{
    public class Rainbow : MonoBehaviour
    {
        [SerializeField] private Vector2 force;
        [SerializeField] private bool haveTrigger;
        [SerializeField]
        private float width, height;
        [SerializeField, ShowIf(nameof(haveTrigger))]
        private Transform start, end;
        [SerializeField, ShowIf(nameof(haveTrigger))]
        private Transform topPlatform;
        [SerializeField, ShowIf(nameof(haveTrigger))]
        private BoxCollider2D platformBoxCollider,rainBowCollider;
        [SerializeField, ShowIf(nameof(haveTrigger))]
        private float speed,flowSpeed;
        [SerializeField, ShowIf(nameof(haveTrigger))]
        private Ease ease;
        [SerializeField] private BuoyancyEffector2D effector;
        [SerializeField] private SpriteRenderer rainbow;
        [SerializeField] private Renderer rainbowRenderer;
        [SerializeField] private SkeletonAnimation animation;
        [SerializeField,SpineAnimation] private string trigger, on,idle;
        private bool isActive;
        private MaterialPropertyBlock block;
        private bool complete;
        private float _offsetY = 0;
        private void Start()
        {
            block = new MaterialPropertyBlock();
        }

        public void ActiveRainBow(string tag,Transform target)
        {
            if(isActive) return;
            isActive = true;
            platformBoxCollider.enabled = false;
            GamePlayController.Instance.controlCharacter.SetVelocity(force);
            //GamePlayController.Instance.ball.SetForceInstant(force);
            var trackEntry = animation.state.SetAnimation(0, trigger, false);
            trackEntry.Complete += entry =>
            {
                animation.state.SetAnimation(0, on, true);
                topPlatform.DOMove(end.position,speed).SetEase(ease).SetSpeedBased(true)
                    .OnUpdate(() =>
                    {
                        var pos = topPlatform.transform.position;
                        var size = rainbow.size;
                        size.y = Mathf.Abs(pos.y- start.position.y+height);
                        rainbow.size = size;
                        var posRainbow = pos;
                        posRainbow.y -= Mathf.Abs(pos.y- start.position.y)/2+platformBoxCollider.size.y/2;
                        rainbow.transform.position = posRainbow ;
                        rainBowCollider.size = size;
                        effector.surfaceLevel = size.y / 2;
                    })
                    .OnComplete(() =>
                    {
                        platformBoxCollider.enabled = true;
                        platformBoxCollider.isTrigger = true;
                        complete = true;
                        animation.state.SetAnimation(0, idle, true);
                    });
            };
        }

        private void Update()
        {
            _offsetY += Time.deltaTime * flowSpeed;
            rainbowRenderer.GetPropertyBlock(block);
            block.SetVector("_MainTex_ST",new Vector4(1,1,0,_offsetY));
            rainbowRenderer.SetPropertyBlock(block);
            
        }
        public void ActivePlatform(string tag,Transform target)
        {
            
            platformBoxCollider.isTrigger = false;
        }
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (haveTrigger)
                {
                    topPlatform.gameObject.SetActive(true);
                    topPlatform.transform.position = start.position;
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(start.position,0.1f);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(end.position,0.1f);
                    var size = rainbow.size;
                    size.y = height;
                    size.x = width;
                    rainbow.size = size;
                    rainbow.transform.position = topPlatform.transform.position - new Vector3(0, height/2+platformBoxCollider.size.y/2, 0);
                    rainBowCollider.size = size;
                    effector.surfaceLevel = size.y / 2;
                }
                else
                {
                    topPlatform.gameObject.SetActive(false);
                    var size = rainbow.size;
                    size.y = height;
                    size.x = width;
                    rainbow.size = size;
                    effector.surfaceLevel = size.y / 2;
                }
            }
        }
    }
}