using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Truongtv.SoundManager
{
    public class BgmAudio: SimpleAudio
    {
        public static BgmAudio Instance;
        private const float FadeInDuration = 0.5f;
        private const float FadeOutDuration = 0.5f;
        private bool started;
        protected override void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SoundManager.OnBgmSettingChange += OnSettingChange;
        }

        protected override void PlaySound()
        {
            source.mute = !SoundManager.IsSoundBgm();
            source.Play();
            SoundFadeIn().Forget();
            started = true;
        }

        public bool IsPlaying()
        {
            return started;
        }
    
        public override void Stop(bool forceStop = false)
        {
            if(!forceStop)
                SoundFadeOut().Forget();
            source.clip = null;
            source.loop = false;
        }

        private async UniTaskVoid SoundFadeIn()
        {
            source.volume = 0;
            var time = 0f;
            while (time < FadeInDuration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Clamp01(time / FadeInDuration);
                await UniTask.Yield();
            }
            source.volume = 1f;
        }

        private  async UniTaskVoid SoundFadeOut()
        {
            source.volume = 1f;
            var time = 0f;
            while (time < FadeOutDuration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Clamp01(1f-time / FadeInDuration);
                await UniTask.Yield();
            }
            source.volume = 0f;
            source.Pause();
        }
    }
}