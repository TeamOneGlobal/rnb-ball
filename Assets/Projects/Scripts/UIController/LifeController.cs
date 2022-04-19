using DG.Tweening;
using Projects.Scripts;
using TMPro;
using Truongtv.Utilities;
using UnityEngine;

namespace UIController
{
    public class LifeController : SingletonMonoBehavior<LifeController>
    {
        [SerializeField] private TextMeshProUGUI totalLifeText;
        [SerializeField] private CanvasGroup addLife;
        [SerializeField] private TextMeshProUGUI addLifeText;

        public void UpdateLife()
        {
            UpdateLifeText();
        }

        public void Addlife(long value)
        {
            var currentLife = GameDataManager.Instance.GetCurrentLife();
            var afterLife = currentLife + value;
            if (value < 0)
            {
                GameDataManager.Instance.LostLife();
            }
            else
            {
                GameDataManager.Instance.AddLife((int) value);
            }

            var transform1 = addLife.transform;
            transform1.localPosition = new Vector3(0, -150, 0);
            transform1.localScale = Vector3.zero;
            if (value > 0)
                addLifeText.text = "+" + value;
            else
                addLifeText.text = "" + value;
            addLife.alpha = 1;
            addLife.gameObject.SetActive(true);
            var sequence = DOTween.Sequence();
            sequence.Append(addLife.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack)
                .SetUpdate(UpdateType.Normal, true));
            sequence.Append(addLife.DOFade(0, 0.5f).SetDelay(0.5f).SetEase(Ease.InQuad)
                .SetUpdate(UpdateType.Normal, true));
            sequence.Join(addLife.transform.DOLocalMoveY(-25f, 0.5f).SetEase(Ease.InQuad)
                .SetUpdate(UpdateType.Normal, true));
            sequence.OnComplete(() => { UpdateLifeText(); });
        }

        private void UpdateLifeText()
        {
            if (GameDataManager.Instance != null)
                totalLifeText.text = $"{GameDataManager.Instance.GetCurrentLife()}";
        }
    }
}