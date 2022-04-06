using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Truongtv.SoundManager
{
    public class SoundManager : MonoBehaviour
    {
        private const string INSERT_KEY = "_number_";
        private const string SOUND_SFX = "sfx";
        private const string SOUND_BGM = "bgm";
        [SerializeField] private SimpleAudio sfxPrefab;
        [SerializeField] private List<SimpleAudio> simpleAudioList;
        private static bool _soundSfx, _soundBgm;
        [SerializeField] private AudioClip buttonClickSound;
        public static Action<bool> OnBgmSettingChange;
        public static Action<bool> OnSfxSettingChange;
        public void Awake()
        {
           // SceneManager.sceneUnloaded += SceneUnloaded;
            _soundSfx = IsSoundSfx();
            _soundBgm = IsSoundBgm();
        }

        public static bool IsSoundSfx()
        {
            return PlayerPrefs.GetInt(SOUND_SFX) ==0;
        }

        public static  bool IsSoundBgm()
        {
            return PlayerPrefs.GetInt(SOUND_BGM) ==0;
        }

        public static void SetSoundSfx(bool isOn)
        {
            _soundSfx = isOn;
            OnSfxSettingChange?.Invoke(isOn);
            PlayerPrefs.SetInt(SOUND_SFX, isOn?0:-1);
            PlayerPrefs.Save();
        }

        public static void SetSoundBgm(bool isOn)
        {
            _soundBgm = isOn;
            OnBgmSettingChange?.Invoke(isOn);
            PlayerPrefs.SetInt(SOUND_BGM, isOn?0:-1);
            PlayerPrefs.Save();
        }
        public void PlaySfx(AudioClip clip, bool isLoop = false,float delay = 0f,Action onComplete = null)
        {
            var simple = GetSfxInstance();
            simple.Play(clip, isLoop,delay,onComplete).Forget();
        }

        public void PlayBgm(AudioClip clip)
        {
            if(!BgmAudio.Instance.IsPlaying())
                BgmAudio.Instance.Play(clip, true).Forget();
            else
            {
                BgmAudio.Instance.Resume();
            }
        }
        public void Pause(bool isPause)
        {
            if(isPause)
                BgmAudio.Instance.Pause();
            else BgmAudio.Instance.Resume();
        }

        public void StopSfx(AudioClip clip)
        {
            for (var i = 0; i < simpleAudioList.Count; i++)
            {
                if (simpleAudioList[i] == null || !simpleAudioList[i].gameObject.activeSelf) continue;
                var currentClip = simpleAudioList[i].CurrentClip();
                if(currentClip==null|| currentClip.Equals(clip))
                    simpleAudioList[i].Stop();
            }
        }

        private SimpleAudio GetSfxInstance()
        {
            if (sfxPrefab == null)
                return null;
            for (var i = 0; i < simpleAudioList.Count; i++)
            {
                if (simpleAudioList[i].gameObject.activeSelf) continue;
                simpleAudioList[i].gameObject.SetActive(true);
                return simpleAudioList[i];
            }
            var count = simpleAudioList.Count;
            var go = Instantiate(sfxPrefab, transform);
            go.transform.SetParent(transform);
            go.gameObject.name = SOUND_SFX + INSERT_KEY + count;
            simpleAudioList.Add(go);
            return go;
        }


        private void SceneUnloaded(Scene scene)
        {
            StopAllSoundEffect();
        }

        private void StopAllSoundEffect()
        {
            for (var i = 0; i < simpleAudioList.Count; i++)
            {
                if(simpleAudioList[i].gameObject.activeSelf)
                    simpleAudioList[i].Stop();
            }
        }
        public void PlayButtonClickSound()
        {
            PlaySfx(buttonClickSound);
        }
    }
}