using Projects.Scripts;
using UnityEngine;

namespace Tutorial
{
    public class TutorialLevel4 : MonoBehaviour
    {
        [SerializeField] private GameObject tutorial,blackScene;
        [SerializeField] private Joystick fakeMoveCam;
        private void Start()
        {
            tutorial.gameObject.SetActive(true);
            fakeMoveCam.onPointDown.AddListener(OnMoveCamera);
            fakeMoveCam.onPointUp.AddListener(OnEndMoveCamera);
        }
        private void OnMoveCamera()
        {
            blackScene.SetActive(false);
        }

        private void OnEndMoveCamera()
        {
            gameObject.SetActive(false);
            GameDataManager.Instance.FinishStep();
        }
    }
}