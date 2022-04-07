using Projects.Scripts;
using Projects.Scripts.UIController;
using UIController;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialLevel7 : MonoBehaviour
    {
        [SerializeField] private Button fakeSwitch;
        [SerializeField] private CustomButton switchCharacter;
        [SerializeField] private GameObject step;
        private void Start()
        {
            if (GameDataManager.Instance.GetTutorialStep() != (int) TutorialStep.Level7)
            {
                gameObject.SetActive(false);
                return;
            }
            fakeSwitch.onClick.AddListener(() =>
            {
                switchCharacter.onClick.Invoke();
                GameDataManager.Instance.FinishStep();
                gameObject.SetActive(false);
            });
        }

        public void ActiveTutorial()
        {
            if(!gameObject.activeSelf) return;
            step.SetActive(true);
        }
    }
}