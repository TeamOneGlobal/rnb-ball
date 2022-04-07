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
        [SerializeField, SpineAnimation] private string[] idles,callAnims, winAnims;
        [SerializeField, SpineAnimation] private string dieAnim;
        private Action _callback;
        private bool _forceStay;

        private CharacterController _controller;
        public void Init(CharacterController controller)
        {
            _controller = controller;
        }
        private void Start()
        {
            var skin = GameDataManager.Instance.GetCurrentSkin();
            GameServiceManager.Instance.logEventManager.LogEvent("skin_used",new Dictionary<string, object>
            {
                { "skin",skin}
            });
            TrySkin(skin);
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

            if (GameDataManager.Instance.skinData.IsSkinPremium(skin))
            {
                PlayAnim("ingame_premium_"+skin,3,true);
            }
            animation.AnimationState.Complete += OnAnimationComplete;
        }
        private void OnAnimationComplete(TrackEntry trackEntry)
        {
            _callback?.Invoke();

        }

        private void PlayAnim(string animName,int trackIndex,bool isLoop = false,Action complete = null)
        {
            animation.state.SetAnimation(trackIndex, animName,isLoop);
            _callback = complete;
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
            for (var i = 0; i < winAnims.Length; i++)
            {
                PlayAnim(winAnims[i],i,true);
            }
        }

        public void PlayCallAnim()
        {
            _forceStay = true;
            for (var i = 0; i < callAnims.Length; i++)
            {
                PlayAnim(callAnims[i],i,true);
            }
        }

        private void Update()
        {
            if(!_forceStay) return;
            target.eulerAngles = Vector3.zero;
        }
    }
}