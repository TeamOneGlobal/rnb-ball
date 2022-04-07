using Projects.Scripts.Data;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace Projects.Scripts.UIController.Menu
{
    public class SpinItem : MonoBehaviour
    {
        [SerializeField] private GameObject  coinObj, lifeObj;
        [SerializeField] private SkeletonGraphic red,blue;
        [SerializeField] private TextMeshProUGUI valueText;
        private SpinItemData _spinItemData;
        
        public void Init(SpinItemData data)
        {
            _spinItemData = data;
            
            coinObj.SetActive(_spinItemData.itemData.coinValue>0);
            lifeObj.SetActive(_spinItemData.itemData.lifeValue>0);
            red.gameObject.SetActive(!string.IsNullOrEmpty(_spinItemData.itemData.skinName));
            blue.gameObject.SetActive(!string.IsNullOrEmpty(_spinItemData.itemData.skinName));
            var value = _spinItemData.itemData.coinValue > 0
                ? _spinItemData.itemData.coinValue
                : _spinItemData.itemData.lifeValue;
            if (value>0)
            {
                valueText.gameObject.SetActive(true);
                valueText.text = $"{value}";
            }

            if (!string.IsNullOrEmpty(_spinItemData.itemData.skinName))
            {
                red.initialSkinName = _spinItemData.itemData.skinName+"_1";
                red.Initialize(true);
                blue.initialSkinName = _spinItemData.itemData.skinName+"_2";
                blue.Initialize(true);
            }
        }
    }
}