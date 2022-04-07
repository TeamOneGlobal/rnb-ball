using Projects.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialMenu : MonoBehaviour
    {
        [SerializeField] private GameObject step1;
        [SerializeField] private Button startButton,fakeButton;
        private void Awake()
        {
            
        }

        private void Start()
        {
            if (GameDataManager.Instance.GetTutorialStep()!= (int) TutorialStep.ClickStart)
            {
                gameObject.SetActive(false);
                return;
            }
            if (GameDataManager.Instance.GetTutorialStep() == (int) TutorialStep.ClickStart)
            {
                startButton.onClick.AddListener(() =>
                {
                    GameDataManager.Instance.FinishStep();
                });
                fakeButton.onClick = startButton.onClick;
                step1.SetActive(true);
            }
        }
    }
}