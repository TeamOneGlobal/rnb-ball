using Projects.Scripts.UIController;
using Truongtv.Utilities;
using UnityEngine;

namespace GamePlay
{
    public class CoinCollector : SingletonMonoBehavior<CoinCollector>
    {
        [SerializeField] private CoinValueText collectPrefab;
        [SerializeField] private Transform collectContainer;
        public long total;
        public void Collect(long value, Vector3 position)
        {
            total += value;
            var item = Instantiate(collectPrefab, collectContainer);
            item.transform.localScale = Vector3.one;
            item.transform.position = position;
            item.SetValue((int)value);
        }
    }
}