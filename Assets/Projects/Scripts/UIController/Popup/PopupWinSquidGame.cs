using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay;
using Projects.Scripts;
using Projects.Scripts.UIController;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.LogManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

public class PopupWinSquidGame : BasePopup
{
    [SerializeField] private Transform container;
    [SerializeField] private Button watchAdButton, closeButton, backButton;
    [SerializeField] private TextMeshProUGUI levelText, rankText, coinText;
    private Action _onClose, _onWatchAd;
    private int rank;
    public void Initialized(int rank, int coin, int level, Action onWatchAd, Action onClose)
    {
        _onClose = onClose;
        _onWatchAd = onWatchAd;
        closeButton.onClick.AddListener(OnClose);
        backButton.onClick.AddListener(OnClose);
        watchAdButton.onClick.AddListener(OnWatchAd);
        levelText.text = $"LEVEL {level}";
        coinText.text = $"+ {coin}";
        rankText.text = "11/11";
        this.rank = rank;
        openAction += () =>
        {
            closeButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            OnStart();
        };
        openCompleteAction += () => { StartCoroutine(AllowClose()); };
        // if (!GameDataManager.Instance.showBannerInGame)
        //     GameServiceManager.Instance.adManager.ShowBanner();
    }

    IEnumerator Ranking(int rank)
    {
        yield return new WaitForSeconds(1);
        int indexRank = 11;
        while (indexRank > rank)
        {
            indexRank--;
            rankText.text = $"{indexRank}/11";
            yield return new WaitForSeconds(.1f);
        }
    }
    
    IEnumerator AllowClose()
    {
        yield return new WaitForSecondsRealtime(2);
        closeButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    private void OnStart()
    {
        container.localScale = Vector3.zero;
        SoundManager.Instance.PlayPopupOpenSound();
        container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                StartCoroutine(Ranking(rank));
            });
    }

    void OnClose()
    {
        // if (!GameDataManager.Instance.showBannerInGame)
        //     GameServiceManager.Instance.adManager.HideBanner();
        closeCompleteAction = null;
        Close();
        _onClose?.Invoke();
    }

    void OnWatchAd()
    {
        GameServiceManager.Instance.adManager.ShowRewardedAd("squidGame_x2_coin", () =>
        {
            GameServiceManager.Instance.logEventManager.LogEvent("count_ad_in_game_finish", new Dictionary<string, object>
            {
                {"level", "lv_" + GamePlayController.Instance.level},
                {"type", "revive"}
            });
            Close();
            _onWatchAd?.Invoke();
        });
    }
}