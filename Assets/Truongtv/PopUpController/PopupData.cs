using UnityEngine;

namespace Truongtv.PopUpController
{
    [CreateAssetMenu(fileName = "PopupData", menuName = "Truongtv/Popup/PopupData", order = 1)]
    public class PopupData:ScriptableObject
    {
        public BasePopup notificationPrefab;
    }
}