using System;
using System.Collections.Generic;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GamePlay.CameraControl;
using GamePlay.Characters;
using MEC;
using Projects.Scripts;
using Projects.Scripts.Data;
using Projects.Scripts.GamePlay.Sound;
using Projects.Scripts.UIController;
using Projects.Scripts.UIController.Popup;
using Sirenix.OdinInspector;
using Spine.Unity;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.Utilities;
using UIController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CharacterController = GamePlay.Characters.CharacterController;
using UpdateType = DG.Tweening.UpdateType;

namespace GamePlay
{
    public class GamePlayController : SingletonMonoBehavior<GamePlayController>
    {
        public int level;
        [SerializeField] private SkeletonGraphic ball;
        [HideInInspector]public CharacterController controlCharacter;
        [HideInInspector]public CharacterController red, blue;
        [SerializeField] private GameObject hand;

        [Title("UI")] [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image[] controlObject;
        [SerializeField] private Image imgChangeCharacter;
        [SerializeField] private Sprite redImg, blueImg;
        [BoxGroup("Button")] [SerializeField] private CustomButton moveLeft, moveRight, jump, changeTarget;
        [BoxGroup("Button"), SerializeField] private Button pauseButton;
        [BoxGroup("Button"), SerializeField] private Joystick joyStick;
        [SerializeField,BoxGroup("Cheat")] private Toggle toggleShowUI;
        [SerializeField, BoxGroup("Cheat")] private Image[] imgList;
        [SerializeField, BoxGroup("Cheat")] private GameObject[] objList;
        [HideInInspector]public GameState gameState;
        private bool _isBlueGateOpen, _isRedGateOpen;
        private bool _changingCharacter;
        private string _skin;
        public override void Awake()
        {
            base.Awake();
            var sceneName = SceneManager.GetActiveScene().name;
            int.TryParse(sceneName.Replace("Level ", ""),out level);
            levelText.text = sceneName;
            var chars = FindObjectsOfType<CharacterController>();
            red = chars.First(a => a.character == Character.Red);
            blue = chars.First(a => a.character == Character.Blue);
            controlCharacter = blue;
            
            ProCamera2D.Instance.RemoveAllCameraTargets();
            ProCamera2D.Instance.AddCameraTarget(blue.transform);
        }

        private void Start()
        {
            if (GameDataManager.Instance.cheated)
            {
                toggleShowUI.gameObject.SetActive(true);
                toggleShowUI.onValueChanged.AddListener(value =>
                {
                    foreach (var img in imgList)
                    {
                        var color = img.color;
                        color.a = value ? 1 : 0;
                        img.color = color;
                    }

                    foreach (var obj in objList)
                    {
                        obj.SetActive(value);
                    }
                });
            }
            else
            {
                toggleShowUI.gameObject.SetActive(false);
            }
            moveLeft.onEnter.AddListener(MoveLeft);
            moveLeft.onExit.AddListener(EndMoveLeft);
            moveRight.onEnter.AddListener(MoveRight);
            moveRight.onExit.AddListener(EndMoveRight);
            jump.onClick.AddListener(Jump);
            changeTarget.onClick.AddListener(SwitchCharacter);
            changeTarget.onClick.AddListener(SoundInGameManager.Instance.PlayChangeTargetSound);
            pauseButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                PopupInGameController.Instance.OpenPopupPause();
            });
            joyStick.onPointDown.AddListener(MoveCamera.Instance.OnPointerDown);
            joyStick.onPointUp.AddListener(MoveCamera.Instance.OnPointerUp);
            joyStick.onDrag.AddListener(MoveCamera.Instance.OnDrag);
            
            MoveCamera.Instance.onStartMove = () =>
            {
                gameState = GameState.Pause;
                controlCharacter.CancelAllMove();
            };
            MoveCamera.Instance.onEndMove = () => { gameState = GameState.Playing; };
            MagneticController.Instance.Init();
            gameState = GameState.Playing;
            SoundInGameManager.Instance.PlayBgmSound();
            LifeController.Instance.UpdateLife();
            _skin = GameDataManager.Instance.GetCurrentSkin();
            ball.initialSkinName = GameDataManager.Instance.GetCurrentSkin()+"_2";
            ball.Initialize(true);
            GameServiceManager.Instance.logEventManager.LogEvent("level_start",new Dictionary<string, object>
            {
                { "level","lv_"+level}
            });
            if (!GameDataManager.Instance.IsPurchaseBlockAd()&& GameDataManager.Instance.showBannerInGame)
            {
                GameServiceManager.Instance.adManager.ShowBanner();
            }
            else
            {
                GameServiceManager.Instance.adManager.HideBanner();
            }
        }
        #region Gate state

        public Action onOpenRed, onOpenBlue;
        public void OpenGate(string playerTag)
        {
            controlCharacter.CancelAllMove();
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
                if (level <= 5)
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
            if (gameState == GameState.Pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            gameState = GameState.Pause;
            LogicalPause();
        }

        public void Resume()
        {
            gameState = GameState.Playing;
            LogicalResume();
        }

        public async UniTaskVoid Win()
        {
            gameState = GameState.End;
            controlCharacter.CancelAllMove();
            controlCharacter.SetVelocity(Vector2.zero);
            pauseButton.gameObject.SetActive(false);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            ForceWin();
        }

        private void ForceWin()
        {
            gameState = GameState.End;
            controlCharacter.CancelAllMove();
            red.PlayWinAnim();
            blue.PlayWinAnim();
            GameServiceManager.Instance.logEventManager.LogEvent("level_complete",new Dictionary<string, object>
            {
                { "level","lv_"+level}
            });
            GameDataManager.Instance.GameResult(GameResult.Win, level, (int)CoinCollector.Instance.total);
            var skin = GameDataManager.Instance.skinData.Skins.Find(a =>
                a.unlockType == UnlockType.Level && a.unlockValue == level);
            if (level >= 3||GameDataManager.Instance.GetCurrentLevel()>3)
            {
                SoundInGameManager.Instance.PlayWinSound(()=>
                {
                    if (skin != null && !string.IsNullOrEmpty(skin.skinName) &&
                        GameDataManager.Instance.GetCurrentLevel() == level)
                    {
                        PopupInGameController.Instance.OpenPopupReceiveSkin(skin.skinName,
                            () =>
                            {
                                GameServiceManager.Instance.adManager.ShowInterstitialAd(LoadSceneController.LoadMenu);
                            });
                    }
                    else
                    {
                        GameServiceManager.Instance.adManager.ShowInterstitialAd(LoadSceneController.LoadMenu);
                    }
                });
            }
            else
            {
                SoundInGameManager.Instance.PlayWinSound(() =>
                {
                    if (skin != null && !string.IsNullOrEmpty(skin.skinName) &&
                        GameDataManager.Instance.GetCurrentLevel() == level)
                    {
                        PopupInGameController.Instance.OpenPopupReceiveSkin(skin.skinName,
                            () =>
                            {
                                GameServiceManager.Instance.adManager.ShowInterstitialAd(() =>
                                {
                                    var lastLevelData = GameDataManager.Instance.GetLastLevelData();
                                    GameDataManager.Instance.UpdateCoin((int)CoinCollector.Instance.total);
                                    GameDataManager.Instance.UpdateCoin(lastLevelData.coins);
                                    GameDataManager.Instance.UpdateLastLevel();
                                    var newLevel = GameDataManager.Instance.GetCurrentLevel();
                                    LoadSceneController.LoadLevel(newLevel);
                                });
                            });
                    }
                    else
                    {
                        GameServiceManager.Instance.adManager.ShowInterstitialAd(() =>
                        {
                            var lastLevelData = GameDataManager.Instance.GetLastLevelData();
                            GameDataManager.Instance.UpdateCoin((int)CoinCollector.Instance.total);
                            GameDataManager.Instance.UpdateCoin(lastLevelData.coins);
                            GameDataManager.Instance.UpdateLastLevel();
                            var newLevel = GameDataManager.Instance.GetCurrentLevel();
                            LoadSceneController.LoadLevel(newLevel);
                        });
                    }
                    
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

        public void Lose()
        {
            GameServiceManager.Instance.logEventManager.LogEvent("level_fail",new Dictionary<string, object>
            {
                { "level","lv_"+level}
            });
            SoundInGameManager.Instance.PlayLoseSound(() =>
            {
                gameState = GameState.End;
                red.CancelAllMove();
                red.SetVelocity(Vector2.zero);
                blue.CancelAllMove();
                blue.SetVelocity(Vector2.zero);
                PopupInGameController.Instance.OpenPopupRevive(SetCharacterRevive, () =>
                {
                    GameDataManager.Instance.GameResult(GameResult.Lose, level, (int)CoinCollector.Instance.total);
                    LogicalResume();
                    LoadSceneController.LoadMenu();
                },_skin);
            });
        }

        public async void CharacterDie()
        {
            gameState = GameState.Pause;
            controlCharacter.CancelAllMove();
            GameServiceManager.Instance.logEventManager.LogEvent("die",new Dictionary<string, object>
            {
                { "level","lv_"+level}
            });
            
            var totalLife = GameDataManager.Instance.GetCurrentLife();
            if (totalLife > 1)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
                LifeController.Instance.Addlife(-1);
                SetCharacterRevive();
            }
            else
                Lose();
        }

        public void SetCharacterRevive()
        {
            controlCharacter.Revive(() => { gameState = GameState.Playing; });
        }

        #endregion


        #region Editor Control

#if UNITY_EDITOR
        private void Update()
        {
            if (gameState != GameState.Playing) return;
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
            if (controlCharacter != null && gameState == GameState.Playing)
                controlCharacter.MoveLeft(true);
        }

        private void MoveRight()
        {
            if (controlCharacter != null && gameState == GameState.Playing)
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
            if (controlCharacter != null && gameState == GameState.Playing)
                controlCharacter.Jump();
        }

        public void SwitchCharacter()
        {
            hand.SetActive(false);
            if (controlCharacter.IsJumping()) return;
            if (gameState != GameState.Playing) return;
            if (_changingCharacter) return;
            if (controlCharacter != null && gameState == GameState.Playing)
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
            imgChangeCharacter.transform.DOLocalRotate(new Vector3(0, 0, 180), 0.35f,RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() =>
                {
                   _changingCharacter  = false;
                });

            if (ProCamera2D.Instance.CameraTargets[0].TargetTransform == red.transform)
            {
                ProCamera2D.Instance.CameraTargets[0].TargetTransform = blue.transform;
                ball.initialSkinName = GameDataManager.Instance.GetCurrentSkin()+"_2";
                ball.Initialize(true);
            }

            else if (ProCamera2D.Instance.CameraTargets[0].TargetTransform == blue.transform)
            {
                ProCamera2D.Instance.CameraTargets[0].TargetTransform = red.transform;
                ball.initialSkinName = GameDataManager.Instance.GetCurrentSkin()+"_1";
                ball.Initialize(true);
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
                gameState = GameState.Pause;
                red.CancelAllMove();
                blue.CancelAllMove();
                DOTween.PauseAll();
                Timing.PauseCoroutines();
            }
            else
            {
                gameState = GameState.Playing;
                LogicalResume();
            }
        }

        #endregion

        
    }
}