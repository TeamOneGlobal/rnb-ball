using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Projects.Scripts.UIController
{
    public class SplashScene : MonoBehaviour
    {
        private async void Start()
        {
            GameDataManager.Instance.LoadUserData();
            var currentLevel = GameDataManager.Instance.GetCurrentLevel();
            // if(currentLevel> GameDataManager.Instance.maxLevel)
            //     LoadSceneController.LoadMenu();
            // else
            // {
            //     LoadSceneController.LoadLevel(currentLevel);
            // }
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            LoadSceneController.LoadMenu();
        }
    }
}