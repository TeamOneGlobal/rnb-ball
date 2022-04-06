using System;
using UIController;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace Tutorial
{
    public class TutorialLevel7 : MonoBehaviour
    {
        [SerializeField] private Button fakeSwitch;
        [SerializeField] private CustomButton switchCharacter;
        [SerializeField] private GameObject step;
        private void Start()
        {
            if (UserDataController.GetTutorialStep() != (int) TutorialStep.Level7)
            {
                gameObject.SetActive(false);
                return;
            }
            fakeSwitch.onClick.AddListener(() =>
            {
                switchCharacter.onClick.Invoke();
                UserDataController.SetFinishStep();
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