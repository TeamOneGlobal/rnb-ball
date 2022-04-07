using TMPro;
using UnityEngine;

namespace Projects.Scripts.UIController
{
    public class CoinValueText:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinValueText;

        public void SetValue(int value)
        {
            coinValueText.text = "+"+value;
        }
        public void Complete(GameObject game)
        {
            DestroyImmediate(game.gameObject);
        }
    }
}