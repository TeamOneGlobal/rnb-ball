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
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            var currentLevel = GameDataManager.Instance.GetCurrentLevel();
            if(currentLevel> GameDataManager.Instance.maxLevel)
                LoadSceneController.LoadMenu();
            else
            {
                LoadSceneController.LoadLevel(currentLevel);
            }
            //
            //LoadSceneController.LoadMenu();
        }
    }
}