using System;
using Cysharp.Threading.Tasks;
using Projects.Scripts;
using Projects.Scripts.UIController;
using UnityEngine;
using UnityEngine.UI;
using GamePlayController = GamePlay.GamePlayController;

namespace Tutorial
{
    public class TutorialGamePlay : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private CustomButton left,right,jump,change;
        
        [SerializeField] private CustomButton fakeLeft,fakeRight,fakeJump,fakeChange;
        [SerializeField] private FixedJoystick moveCam, fakeMoveCam;
        [SerializeField] private GameObject step1, step2, step3, step4,step5,blackScene,handStep5;

        private async void Start()
        {
            if (GameDataManager.Instance.GetTutorialStep() != (int) TutorialStep.Level1)
            {
                gameObject.SetActive(false);
                return;
            }
            pauseButton.gameObject.SetActive(false);
            
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            jump.gameObject.SetActive(false);
            change.gameObject.SetActive(false);
            moveCam.gameObject.SetActive(false);
            
            right.onEnter.AddListener(DisableBlackScene);
            fakeRight.onClick = right.onClick;
            fakeRight.onEnter = right.onEnter;
            fakeRight.onExit = right.onExit;
            
            left.onEnter.AddListener(DisableBlackScene);
            fakeLeft.onClick = left.onClick;
            fakeLeft.onEnter = left.onEnter;
            fakeLeft.onExit = left.onExit;
            
            jump.onClick.AddListener(OnJumpClick);
            fakeJump.onClick = jump.onClick;
            fakeJump.onEnter = jump.onEnter;
            fakeJump.onExit = jump.onExit;
            
            change.onClick.AddListener(OnChangeClick);
            fakeChange.onClick = change.onClick;
            fakeChange.onEnter = change.onEnter;
            fakeChange.onExit = change.onExit;
            
            fakeMoveCam.onPointDown.AddListener(OnMoveCamera);
            fakeMoveCam.onPointUp.AddListener(OnEndMoveCamera);

            GamePlayController.Instance.onOpenBlue = OnBlueOpenDoor;
            GamePlayController.Instance.onOpenRed = () =>
            {
                GameDataManager.Instance.FinishStep();
                step4.SetActive(false);
            };
            StartStep1();
        }

        private void DisableBlackScene()
        {
            blackScene.gameObject.SetActive(false);
        }

        private void OnMoveCamera()
        {
            DisableBlackScene();
            handStep5.SetActive(false);
        }

        private void OnEndMoveCamera()
        {
            step5.SetActive(false);
            Invoke(nameof(StartStep1),1f);
        }

        private void StartStep1()
        {
            blackScene.gameObject.SetActive(true);
            step1.SetActive(true);
            step2.SetActive(false);
            step3.SetActive(false);
            step4.SetActive(false);
            step5.SetActive(false);
        }
        
        private async void OnBlueOpenDoor()
        {
            GamePlayController.Instance.controlCharacter.CancelAllMove();
            step1.SetActive(false);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            blackScene.gameObject.SetActive(true);
            step2.SetActive(true);
        }

        private void OnJumpClick()
        {
            DisableBlackScene();
            step3.SetActive(false);
            Invoke(nameof(MoveStep4),1.3f);
        }

        private void OnChangeClick()
        {
            DisableBlackScene();
            step2.SetActive(false);
            Invoke(nameof(MoveStep3),1f);
        }
        private void MoveStep4()
        {
            blackScene.gameObject.SetActive(true);
            step1.SetActive(false);
            step2.SetActive(false);
            step3.SetActive(false);
            step4.SetActive(true);
            step5.SetActive(false);
            
        }

        private void MoveStep3()
        {
            blackScene.gameObject.SetActive(true);
            step1.SetActive(false);
            step2.SetActive(false);
            step3.SetActive(true);
            step4.SetActive(false);
            step5.SetActive(false);
        }
    }
}