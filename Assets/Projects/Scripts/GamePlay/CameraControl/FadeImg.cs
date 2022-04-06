using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.CameraControl
{
    public class FadeImg : MonoBehaviour
    {
        [SerializeField]private Image image;

       
        public void FadeIn(float duration)
        {
            image.DOKill();
            image.DOFade(1, duration)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, true);
        }

        public void FadeOut(float duration)
        {
            image.DOKill();
            image.DOFade(0, duration)
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, true);
        }
    }
}