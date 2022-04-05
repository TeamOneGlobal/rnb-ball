using System;
using Cysharp.Threading.Tasks;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Misson
{
    public class MissionItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private GameObject passedObj, comingSoonObj;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Color currentColor, lockColor,unlockColor;
        private int _level;

        public void Init(int level)
        {
            _level = level;
            levelText.text = $"{_level}";
            var maxLevel = UserDataController.GetCurrentLevel();
            toggle.onValueChanged.RemoveAllListeners();
            if (maxLevel == _level)
            {
                gameObject.SetActive(true);
                passedObj.SetActive(false);
                comingSoonObj.SetActive(false);
                toggle.interactable = true;
                levelText.color = currentColor;
                toggle.isOn = true;
            }
            else if (_level < maxLevel)
            {
                gameObject.SetActive(true);
                passedObj.SetActive(true);
                comingSoonObj.SetActive(false);
                toggle.interactable = true;
                levelText.color = unlockColor;
                toggle.isOn = false;
            }
            else if(_level >= maxLevel+1 && _level<=Config.MAX_LEVEL)
            {
                gameObject.SetActive(true);
                passedObj.SetActive(false);
                comingSoonObj.SetActive(false);
                toggle.interactable = false;
                levelText.color = lockColor;
                toggle.isOn = false;
            }
            else if(_level == Config.MAX_LEVEL+1)
            {
                gameObject.SetActive(true);
                passedObj.SetActive(false);
                comingSoonObj.SetActive(true);
                toggle.interactable = false;
                levelText.color = lockColor;
                toggle.isOn = false;
            }
            else
            {
                toggle.isOn = false;
                gameObject.SetActive(false);
            }
            
            toggle.onValueChanged.AddListener(OnClick);
        }

        private async void OnClick(bool isChoose)
        {
            Debug.Log("click level = "+_level);
            if(!isChoose ) return;
            SoundMenuController.Instance.PlayButtonClickSound();
            await UniTask.Delay(TimeSpan.FromMilliseconds(100));
            LoadSceneController.LoadLevel(_level);
        }
    }
}