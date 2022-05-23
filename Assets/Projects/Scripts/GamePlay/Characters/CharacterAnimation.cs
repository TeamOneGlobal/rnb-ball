using System;
using System.Collections.Generic;
using Projects.Scripts;
using Spine;
using Spine.Unity;
using ThirdParties.Truongtv;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Characters
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private new SkeletonAnimation animation;
        [SerializeField, SpineAnimation] private string[] idles;
        [SerializeField, SpineAnimation] private string dieAnim,callAnim,winAnim;
        private Action _callback;
        private bool _forceStay;

        private CharacterController _controller;
        public void Init(CharacterController controller)
        {
            _controller = controller;
        }

        private void Start()
        {
            
            
            if (GameDataManager.Instance != null)
            {
                var skin = GameDataManager.Instance.GetCurrentSkin();
                TrySkin(skin);
                GameServiceManager.Instance.logEventManager.LogEvent("skin_used",new Dictionary<string, object>
                {
                    { "skin",skin}
                });
                
            }
            
        }

        public void TrySkin(string skin)
        {
            if (gameObject.tag.Equals(TagManager.BLUE_TAG))
            {
                animation.initialSkinName = skin + "_2";
            }
            else
            {
                animation.initialSkinName = skin + "_1";
            }
            animation.Initialize(true);
            PlayIdle();

        }

        private void PlayAnim(string animName,int trackIndex,bool isLoop = false,Action complete = null)
        {
            var trackEntry = animation.state.SetAnimation(trackIndex, animName,isLoop);
            trackEntry.Complete += entry =>
            {
                complete?.Invoke();
            }; 
        }

        public void PlayIdle()
        {
            _forceStay = false;
            var r = Random.Range(0, idles.Length);
            PlayAnim(idles[r],0,complete:PlayIdle);
        }

        public void PlayDie(Action oncomplete = null)
        {
            _forceStay = false;
            PlayAnim(dieAnim, 0,complete: oncomplete);
        }

        public void PlayWinAnim()
        {
            _forceStay = true;
            PlayAnim(winAnim,0,true);
        }

        public void PlayCallAnim()
        {
            _forceStay = true;
            PlayAnim(callAnim,0,true);
        }

        private void Update()
        {
            if(!_forceStay) return;
            target.eulerAngles = Vector3.zero;
        }
    }
}