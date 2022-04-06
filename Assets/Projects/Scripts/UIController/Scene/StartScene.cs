using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController
{
    public class StartScene : MonoBehaviour
    {
        [SerializeField] private RectTransform icon;
        [SerializeField] private Image text;
        [SerializeField] private AudioSource source;

        private void Awake()
        {
            icon.localPosition = new Vector3(0, 800, 0);
            text.color = new Color(1, 1, 1, 0);
        }

        private async void Start()
        {
            Application.targetFrameRate = 300;
            UserDataController.LoadUserData();
            icon.DOLocalMoveY(110f, 1f)
                .SetEase(Ease.OutQuart)
                .OnComplete(() =>
                {
                    text.DOFade(1, 2)
                        .SetEase(Ease.InCubic)
                        .OnComplete(async () =>
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(2f));
                            LoadSceneController.LoadMenu();
                        });
                });

            await UniTask.Delay(TimeSpan.FromMilliseconds(450));
            source.Play();
        }
    }
}