using DG.Tweening;
using UnityEngine;

namespace Projects.Scripts.UIController.Menu
{
    public class ShopMenuToggle : ToggleHelper
    {
        [SerializeField] private Transform target;
        [SerializeField] private float yOn,yOff,duration;
        protected override void Awake()
        {
            
            _toggle.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    OnValueChange(true,0f);
                    target.DOLocalMoveY(yOn, duration).SetEase(Ease.InOutSine);
                }
                else
                {
                    target.DOLocalMoveY(yOff, duration).SetEase(Ease.InOutSine).OnComplete(() =>
                    {
                        OnValueChange(false,0f);
                    });;
                }
                
            });
            target.localPosition = new Vector3(0,_toggle.isOn?yOn:yOff,0);
            OnValueChange(_toggle.isOn,0f);
        }
    }
}