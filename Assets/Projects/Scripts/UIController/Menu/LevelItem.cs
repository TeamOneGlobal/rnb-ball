using System;
using Com.LuisPedroFonseca.ProCamera2D;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] private GameObject levelPass, levelNew, levelBoss,bossObj,starObj;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private GameObject[] stars;
        [SerializeField] private Button playButton;
        private int _level;
        private void Start()
        {
            playButton.onClick.AddListener(Play);
        }

        public void Init(int level,int currentLevel,int star,bool isBossLevel)
        {
            _level = level;
            levelPass.SetActive(!isBossLevel||_level < currentLevel);
            levelNew.SetActive(_level > currentLevel);
            levelBoss.SetActive(isBossLevel && _level >= currentLevel);
            bossObj.SetActive(isBossLevel && _level >= currentLevel);
            starObj.SetActive(_level < currentLevel);
            levelText.text = $"{_level}";
            levelText.gameObject.SetActive(_level <= currentLevel);
            for (var i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(i<star);
            }

            playButton.interactable = GameDataManager.Instance.cheated || currentLevel >= _level;
        }
        private void Play()
        {
            SoundManager.Instance.PlayButtonSound();
            if (!GameDataManager.Instance.CanPlayWithoutInternet())
            {
                PopupController.Instance.ShowNoInternet();
                return;
            }
            LoadSceneController.LoadLevel(_level);
        }
    }

}
