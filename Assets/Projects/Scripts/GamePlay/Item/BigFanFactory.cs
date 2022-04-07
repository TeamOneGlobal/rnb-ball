using DG.Tweening;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Item
{
    public class BigFanFactory : MonoBehaviour
    {
        public bool close;
        [SerializeField] private GameObject effect;
        [SerializeField] private SpriteRenderer line;
        [SerializeField] private Transform spin;
        [SerializeField] private Collider2D collider;
        [SerializeField] private SimpleAudio audio;
        private void Start()
        {
            if (close)
            {
                Close();
            }
            else
            {
                var sequence = DOTween.Sequence();
                effect.SetActive(true);
                spin.DOScale(new Vector3(-1, 1, 1), 0.05f)
                    .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                sequence.Append(line.DOFade(1, 1f));
                sequence.OnComplete(() => { collider.enabled = true; });
                sequence.Play();
            }
        }

        [Button]
        public void Open()
        {
            var sequence = DOTween.Sequence();
            effect.SetActive(true);
            audio.Play();
            spin.DOScale(new Vector3(-1, 1, 1), 0.05f)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            sequence.Append(line.DOFade(1, 1f));
            sequence.OnComplete(() => { collider.enabled = true; });
            sequence.Play();
        }
        public void Close()
        {
            var sequence = DOTween.Sequence();
            effect.SetActive(false);
            spin.DOKill();
            sequence.Append(line.DOFade(0, 1f));
            sequence.OnComplete(() => { collider.enabled = false; });
            sequence.Play();
        }
    }
}