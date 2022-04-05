using DG.Tweening;
using TMPro;
using Truongtv.Utilities;
using UnityEngine;

namespace GamePlay
{
    public class CoinCollector : SingletonMonoBehavior<CoinCollector>
    {
        [SerializeField] private TextMeshProUGUI collectPrefab;
        [SerializeField] private Transform collectContainer;
        public long total;
        public void Collect(long value, Vector3 position)
        {
            total += value;
            var item = Instantiate(collectPrefab, collectContainer);
            item.transform.localScale = Vector3.one;
            item.transform.position = position;
            item.text = "+" + value;
            var sequence = DOTween.Sequence();
            sequence.Append(item.DOFade(0, 1f));
            sequence.Join(item.transform.DOMoveY(position.y + 0.5f, 1f));
            sequence.OnComplete(() => { Destroy(item.gameObject); });
            sequence.Play();
        }
    }
}