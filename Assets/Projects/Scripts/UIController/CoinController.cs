using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Truongtv.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;
using Random = UnityEngine.Random;

namespace UIController
{
    public class CoinController : SingletonMonoBehavior<CoinController>
    {
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Transform coinContainer,coinTarget;
        [SerializeField] private GameObject coinPrefab;
        private long _currentCoin;

        public void UpdateCoin()
        {
            _currentCoin = UserDataController.GetCurrentCoin();
            coinText.text = "" + _currentCoin;
        }

        [Button]
        public void DecreaseCoin(long value)
        {
            var newValue = _currentCoin - value;
            UserDataController.UpdateCoin(-value);
            DOTween.To(() => _currentCoin, x => _currentCoin = x,newValue,0.5f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                coinText.text = "" + _currentCoin;
            })
                .OnComplete(() =>
                {
                    _currentCoin = newValue;
                    coinText.text = "" + _currentCoin;
                });
        }
        [Button]
        public async void IncreaseCoin(Transform target,long value,Action callback =null)
        {
            UserDataController.UpdateCoin(value);
            var newValue = _currentCoin + value;
            
            if (target != null)
            {
                int totalCoin;
                if (value < 100)
                {
                    totalCoin = 8;
                }
                else if (value < 1000)
                {
                    totalCoin = 20;
                }
                else
                {
                    totalCoin = 30;
                }

                for (var i = 0; i < totalCoin; i++)
                {
                    var obj = Instantiate(coinPrefab, coinContainer);
                    var position = target.transform.position;
                    obj.transform.position = new Vector3(position.x+Random.Range(-1f,1f),position.y+Random.Range(-1f,1f),position.z);
                    var tween = DOTween.Sequence();
                    var position1 = coinTarget.transform.position;
                    tween.Append(obj.transform.DOMoveX(position1.x, 1.5f).SetEase(Ease.InBack));
                    tween.Join(obj.transform.DOMoveY(position1.y, 1.5f).SetEase(Ease.InQuad));
                    tween.Join(obj.GetComponent<Image>().DOFade(0, 1.5f).SetEase(Ease.Linear));
                    tween.OnComplete(() => { Destroy(obj); });
                    tween.Play();
                }
                await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            }
            
            DOTween.To(() => _currentCoin, x => _currentCoin = x,newValue,0.5f).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    coinText.text = "" + _currentCoin;
                })
                .OnComplete(() =>
                {
                    _currentCoin = newValue;
                    coinText.text = "" + _currentCoin;
                    callback?.Invoke();
                });
        }
    }
}