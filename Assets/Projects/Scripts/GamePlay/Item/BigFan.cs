using DG.Tweening;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Item
{
    public class BigFan : MonoBehaviour
    {
        public bool close;
        [SerializeField] private GameObject effect;
        [SerializeField] private SpriteRenderer line;
        [SerializeField] private Transform cover;
        [SerializeField] private Collider2D collider;
        [SerializeField] private float openY, closeY;
        [SerializeField] private SimpleAudio audio;

        private void Start()
        {
            if (close)
            {
                collider.enabled = false;
                effect.SetActive(false);
                line.DOFade(0, 0f);
                cover.DOLocalMoveY(closeY, 0f);
            }
        }

        public void Open()
        {
            collider.enabled = true;
            var sequence = DOTween.Sequence();
            sequence.Append(cover.DOLocalMoveY(openY, 0.5f).OnComplete(() =>
            {
                effect.SetActive(true);
                audio.Play();
            }));
            sequence.Append(line.DOFade(1, 1f));
            sequence.OnComplete(() => {  });
            sequence.Play();
        }
        public void Close()
        {
            collider.enabled = false;
            var sequence = DOTween.Sequence();
            sequence.Append(cover.DOLocalMoveY(closeY, 0.5f).OnComplete(() =>
            {
                effect.SetActive(false);
                audio.Play();
            }));
            sequence.Append(line.DOFade(0, 1f));
            sequence.OnComplete(() => {  });
            sequence.Play();
        }
    }
}