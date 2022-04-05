using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Truongtv.SoundManager
{
    public class SimpleAudio : MonoBehaviour
    {
        [SerializeField] private protected AudioSource source;
        [SerializeField] private bool uiAudio;
        [SerializeField] public bool autoPlay;
        private bool _isPause;
        private bool _isPlay;


        protected virtual void Awake()
        {
            SoundManager.OnSfxSettingChange += OnSettingChange;
        }

        private void Start()
        {
            if (autoPlay)
            {
                PlaySound();
            }
        }
        private void OnDestroy()
        {
            SoundManager.OnSfxSettingChange -= OnSettingChange;
            SoundManager.OnBgmSettingChange -= OnSettingChange;
        }

        public AudioClip CurrentClip()
        {
            return source.clip;
        }
        public async UniTaskVoid Play(AudioClip playClip, bool isLoop = false,float delay = 0f,Action onComplete = null)
        {
            source.Stop();
            source.clip = playClip;
            source.loop = isLoop;
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: this.GetCancellationTokenOnDestroy());
            if(source==null) return;
            if(!_isPause) PlaySound();
            if (isLoop) return;
            await UniTask.WaitUntil(() => source==null||source.isPlaying==false, cancellationToken: this.GetCancellationTokenOnDestroy());
            onComplete?.Invoke();
            if(gameObject==null|| source==null) return;
            if(gameObject.activeSelf && uiAudio)
                Stop();
        }

        public async UniTaskVoid Play( bool isLoop = false)
        {
            source.Stop();
            source.loop = isLoop;
            if(source==null) return;
            if(!_isPause) PlaySound();
        }
        protected void OnSettingChange(bool isOn)
        {
            source.mute = !isOn;
        }

       
        protected virtual void PlaySound()
        {
            if(source ==null) return;
            source.mute = !SoundManager.IsSoundSfx();
            source.Play();
        }
        public virtual void Stop(bool forceStop = false)
        {
            source.Stop();
            source.loop = false;
            if(uiAudio)
                gameObject.SetActive(false);
        }
        public void Pause()
        {
            _isPause = true;
            source.Pause();
        }

        public void Resume()
        {
            _isPause = false;
            source.UnPause();
        }
    
    }
}