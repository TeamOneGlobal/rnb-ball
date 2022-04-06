using Scriptable;
using TMPro;
using UnityEngine;

namespace UIController.SpinController
{
    public class SpinItem : MonoBehaviour
    {
        private SpinItemData _spinItemData;
        [SerializeField] private GameObject skinObj, otherObj, coinObj, heartObj,spinObj;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private SkeletonGraphicController red, blue;


        public void Init(SpinItemData data)
        {
            _spinItemData = data;
            switch (_spinItemData.type)
            {
                case ItemType.Coin:
                    skinObj.SetActive(false);
                    otherObj.SetActive(true);
                    coinObj.SetActive(true);
                    heartObj.SetActive(false);
                    spinObj.SetActive(false);
                    valueText.text = "+" + _spinItemData.value;
                    break;
                case ItemType.Heart:
                    skinObj.SetActive(false);
                    otherObj.SetActive(true);
                    coinObj.SetActive(false);
                    heartObj.SetActive(true);
                    spinObj.SetActive(false);
                    valueText.text = "+" + _spinItemData.value;
                    break;
                case ItemType.Skin:
                    skinObj.SetActive(true);
                    otherObj.SetActive(false);
                    coinObj.SetActive(false);
                    heartObj.SetActive(false);
                    spinObj.SetActive(false);
                    red.skeleton.initialSkinName = _spinItemData.value + "_1";
                    red.skeleton.Initialize(true);
                    blue.skeleton.initialSkinName = _spinItemData.value + "_2";
                    blue.skeleton.Initialize(true);
                    break;
            }
        }
    }
}