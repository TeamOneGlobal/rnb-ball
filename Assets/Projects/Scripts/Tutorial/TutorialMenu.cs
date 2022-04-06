using System;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

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
            if (UserDataController.GetTutorialStep() != (int) TutorialStep.ClickStart)
            {
                gameObject.SetActive(false);
                return;
            }
            if (UserDataController.GetTutorialStep() == (int) TutorialStep.ClickStart)
            {
                startButton.onClick.AddListener(() =>
                {
                    UserDataController.SetFinishStep();
                });
                fakeButton.onClick = startButton.onClick;
                step1.SetActive(true);
            }
        }
    }
}