using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class MissionItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        private int _level;
        private bool _isOn;
        public void Init(int level)
        {
            _level = level;
            var currentLevel = GameDataManager.Instance.cheated?GameDataManager.Instance.maxLevel:GameDataManager.Instance.GetCurrentLevel();
            button.onClick.RemoveAllListeners();
            if (_level < currentLevel)
            {
                button.onClick.AddListener(OnClick);
            }
            else if (_level >= currentLevel + 1 && _level <= GameDataManager.Instance.maxLevel)
            {
                
            }
            else if (_level == GameDataManager.Instance.maxLevel + 1)
            {
                
            }
            else if (_level == GameDataManager.Instance.maxLevel)
            {
                
            }
        }
        private void OnClick()
        {
            SoundManager.Instance.PlayButtonSound(() =>
            {
                LoadSceneController.LoadLevel(_level);
            });
        }
    }
}