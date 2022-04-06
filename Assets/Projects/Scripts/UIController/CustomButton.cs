using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIController
{
    public class CustomButton : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private Transform targetGraphic;
        [HideInInspector]public UnityEvent onClick,onEnter,onExit;
        private bool _isEnter;
        public void OnPointerDown(PointerEventData eventData)
        {
            
            targetGraphic.DOKill();
            targetGraphic.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.1f).SetEase(Ease.InOutSine)
                .SetUpdate(UpdateType.Normal, true);
            onClick.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter.Invoke();
            if(_isEnter) return;
            _isEnter = true;
            targetGraphic.DOKill();
            targetGraphic.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.1f).SetEase(Ease.InOutSine)
                .SetUpdate(UpdateType.Normal, true);
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit.Invoke();
            _isEnter = false; 
            targetGraphic.DOKill();
            targetGraphic.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.InOutSine)
                .SetUpdate(UpdateType.Normal, true);
            
            
        }

    }
}