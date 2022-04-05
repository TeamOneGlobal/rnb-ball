using System;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GamePlay.CameraControl;
using MEC;
using Scriptable;
using Sirenix.OdinInspector;
using Sound;
using TMPro;
using Truongtv.Services.Ad;
using Truongtv.Services.Firebase;
using Truongtv.Utilities;
using UIController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UserDataModel;
using CharacterController = GamePlay.Characters.CharacterController;
using UpdateType = DG.Tweening.UpdateType;

namespace GamePlay
{
    public class GamePlayController : SingletonMonoBehavior<GamePlayController>
    {
        public int level;

        [SerializeField] public CharacterController controlCharacter;
        [SerializeField] public CharacterController red, blue;
        [SerializeField] private GameObject hand;

        [Title("UI")] [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image[] controlObject;
        [SerializeField] private Image imgChangeCharacter;
        [SerializeField] private Sprite redImg, blueImg;
        [SerializeField] public SkinData skinData;
        [BoxGroup("Button")] [SerializeField] private CustomButton moveLeft, moveRight, jump, changeTarget;
        [BoxGroup("Button"), SerializeField] private Button pauseButton;
        [BoxGroup("Button"), SerializeField] private Joystick joyStick;
        private GameState _gameState;
        private bool _isBlueGateOpen, _isRedGateOpen;
        private bool _changingCharacter;

        public override void Awake()
        {
#if UNITY_EDITOR
            UserDataController.LoadUserData();
#endif
            base.Awake();
            var sceneName = SceneManager.GetActiveScene().name;
            level = int.Parse(sceneName.Replace("Level ", ""));
            levelText.text = sceneName;
            ProCamera2D.Instance.RemoveAllCameraTargets();
            ProCamera2D.Instance.AddCameraTarget(blue.transform);
        }

        private void Start()
        {
            moveLeft.onEnter.AddListener(MoveLeft);
            moveLeft.onExit.AddListener(EndMoveLeft);
            moveRight.onEnter.AddListener(MoveRight);
            moveRight.onExit.AddListener(EndMoveRight);
            jump.onClick.AddListener(Jump);
            changeTarget.onClick.AddListener(SwitchCharacter);
            changeTarget.onClick.AddListener(SoundGamePlayController.Instance.PlayChangeTargetSound);
            pauseButton.onClick.AddListener(Pause);
            joyStick.onPointDown.AddListener(MoveCamera.Instance.OnPointerDown);
            joyStick.onPointUp.AddListener(MoveCamera.Instance.OnPointerUp);
            joyStick.onDrag.AddListener(MoveCamera.Instance.OnDrag);
            MoveCamera.Instance.onStartMove = () =>
            {
                _gameState = GameState.Pause;
                controlCharacter.CancelAllMove();
            };
            MoveCamera.Instance.onEndMove = () => { _gameState = GameState.Playing; };

            LifeController.Instance.UpdateLife();
            MagneticController.Instance.Init();
            _gameState = GameState.Playing;
            var skin = UserDataController.GetSelectedSkin();
            FirebaseLogEvent.LogSkinUsed(skin);
            FirebaseLogEvent.LogLevelStart(level); 
            if ((level-Config.SHOW_REVIEW_AFTER_LEVEL)%Config.SHOW_REVIEW_PER_LEVEL ==0 && !UserDataController.IsRating())
            {
#if UNITY_ANDROID || UNITY_EDITOR
                RatingHelper.RequestReviewInfo();
#endif
            }
            
        }

        #region Gate state

        public Action onOpenRed, onOpenBlue;
        public void OpenGate(string playerTag)
        {
            if (playerTag.Equals(TagManager.RED_TAG))
            {
                _isRedGateOpen = true;
                onOpenRed?.Invoke();
                
            }

            if (playerTag.Equals(TagManager.BLUE_TAG))
            {
                _isBlueGateOpen = true;
                onOpenBlue?.Invoke();
            }

            if (_isBlueGateOpen && _isRedGateOpen)
            {
                Win().Forget();
            }
            else
            {
                if (level > 1 && level <= 5)
                {
                    hand.SetActive(true);
                }
            }
        }

        public void CloseGate(string playerTag)
        {
            if (playerTag.Equals(TagManager.RED_TAG))
                _isRedGateOpen = false;
            if (playerTag.Equals(TagManager.BLUE_TAG))
                _isBlueGateOpen = false;
        }

        #endregion


        #region Game State

        public void TogglePause()
        {
            if (_gameState == GameState.Pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private void Pause()
        {
            _gameState = GameState.Pause;
            LogicalPause();
            GamePlayPopupController.Instance.ShowPausePopup();
        }

        private void Resume()
        {
            _gameState = GameState.Playing;
            LogicalResume();
        }

        public async UniTaskVoid Win()
        {
            _gameState = GameState.End;
            controlCharacter.CancelAllMove();
           
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            ForceWin();
        }

        private void ForceWin()
        {
            _gameState = GameState.End;
            controlCharacter.CancelAllMove();
            red.PlayWinAnim();
            blue.PlayWinAnim();
            FirebaseLogEvent.LogLevelWin(level);
            if (level >= 3||UserDataController.GetCurrentLevel()>3)
            {
                UserDataController.SetLevelWin(level, CoinCollector.Instance.total);
                SoundGamePlayController.Instance.PlayWinSound(()=>
                {
                    NetWorkHelper.ShowInterstitialAd(result =>
                    {
                        LoadSceneController.LoadMenu();
                    });
                });
            }
            else
            {
                SoundGamePlayController.Instance.PlayWinSound(() =>
                {
                   

                    NetWorkHelper.ShowInterstitialAd(result =>
                    {
                        UserDataController.UpdateCoin(CoinCollector.Instance.total);
                        var maxLevel = UserDataController.UpdateLevel(level);
                        FirebaseLogEvent.SerUserMaxLevel(maxLevel);
                        UserDataController.ClearPreviousLevelData();
                        var newLevel = UserDataController.GetCurrentLevel();
                        LoadSceneController.LoadLevel(newLevel);
                    });
                        
                });
            }
        }

        public void LogicalResume()
        {
            Time.timeScale = 1f;
            DOTween.PlayAll();
            Timing.ResumeCoroutines();
        }

        private void LogicalPause()
        {
            Time.timeScale = 0f;
            DOTween.PauseAll();
            Timing.PauseCoroutines();
        }

        [Button]
        public void Lose()
        {
            FirebaseLogEvent.LogLevelLose(level);
            SoundGamePlayController.Instance.PlayLoseSound(() =>
            {
                _gameState = GameState.End;
                controlCharacter.CancelAllMove();
                LogicalPause();
                GamePlayPopupController.Instance.ShowLosePopup();
            });
        }

        public async void CharacterDie()
        {
            _gameState = GameState.Pause;
            controlCharacter.CancelAllMove();
            await UniTask.Delay(TimeSpan.FromSeconds(2.5f));
            var totalLife = UserDataController.GetTotalLife();
            if (totalLife > 1)
            {
                LifeController.Instance.Addlife(-1);
                SetCharacterRevive();
            }
            else
                Lose();
        }

        public void SetCharacterRevive()
        {
            controlCharacter.Revive(() => { _gameState = GameState.Playing; });
        }

        #endregion


        #region Editor Control

#if UNITY_EDITOR
        private void Update()
        {
            if (_gameState != GameState.Playing) return;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft();
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                EndMoveLeft();
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight();
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                EndMoveRight();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchCharacter();
            }
        }
#endif

        #endregion

        #region Controller

        private void MoveLeft()
        {
            if (controlCharacter != null && _gameState == GameState.Playing)
                controlCharacter.MoveLeft(true);
        }

        private void MoveRight()
        {
            if (controlCharacter != null && _gameState == GameState.Playing)
                controlCharacter.MoveRight(true);
        }

        private void EndMoveLeft()
        {
            controlCharacter.MoveLeft(false);
        }

        private void EndMoveRight()
        {
            controlCharacter.MoveRight(false);
        }

        private void Jump()
        {
            if (controlCharacter != null && _gameState == GameState.Playing)
                controlCharacter.Jump();
        }

        public void SwitchCharacter()
        {
            hand.SetActive(false);
            if (controlCharacter.IsJumping()) return;
            if (_gameState != GameState.Playing) return;
            if (_changingCharacter) return;
            if (controlCharacter != null && _gameState == GameState.Playing)
                controlCharacter.CancelAllMove();
            controlCharacter.OnCharacterSelected(false);
            if (controlCharacter == red)
            {
                controlCharacter = blue;
                foreach (var obj in controlObject)
                {
                    obj.color = Color.cyan;
                }

                imgChangeCharacter.sprite = blueImg;
            }
            else if (controlCharacter == blue)
            {
                controlCharacter = red;
                foreach (var obj in controlObject)
                {
                    obj.color = Color.red;
                }

                imgChangeCharacter.sprite = redImg;
            }

            controlCharacter.OnCharacterSelected(true);
            _changingCharacter = true;
            imgChangeCharacter.transform.DORotate(new Vector3(0, 0, 180), 0.35f)
                .SetRelative(true)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() => { _changingCharacter = false; });

            if (ProCamera2D.Instance.CameraTargets[0].TargetTransform == red.transform)
            {
                ProCamera2D.Instance.CameraTargets[0].TargetTransform = blue.transform;
            }

            else if (ProCamera2D.Instance.CameraTargets[0].TargetTransform == blue.transform)
            {
                ProCamera2D.Instance.CameraTargets[0].TargetTransform = red.transform;
            }
        }

        #endregion

        #region UI

        #endregion

        #region Items

        public void MagnetCallback(bool active)
        {
            red.ActiveMagnetic(active);
            blue.ActiveMagnetic(active);
        }

        #endregion

        #region Platform Effect

        public void PauseForCinematic(bool isPause)
        {
            if (isPause)
            {
                _gameState = GameState.Pause;
                controlCharacter.CancelAllMove();
                LogicalPause();
            }
            else
            {
                _gameState = GameState.Playing;
                LogicalResume();
            }
        }

        #endregion

        #region private

#if UNITY_EDITOR
        [Button]
        void SetUpBall()
        {
            var balls = FindObjectsOfType<CharacterController>().ToList();
            controlCharacter = balls.Find(a => a.name.Equals("BlueBall"));
            blue = balls.Find(a => a.name.Equals("BlueBall"));
            red = balls.Find(a => a.name.Equals("RedBall"));
            UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        }
#endif

        #endregion

        private int countClick = 0;
        public void UnlockAllLevel()
        {
            countClick++;
            if (countClick >= 10)
            {
                UserDataController.ForceUpdateLevel();
            }
        }
    }
}