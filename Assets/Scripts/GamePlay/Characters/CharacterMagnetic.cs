using UnityEngine;

namespace GamePlay.Characters
{
    public class CharacterMagnetic:MonoBehaviour
    {
        [SerializeField] private GameObject magnetic;
        public GameObject magnetEffect;
        private int _currentCoinCollect;
        private CharacterController _controller;
        public void Init(CharacterController controller)
        {
            _controller = controller;
        }
        public void ActiveMagnetic(bool active)
        {
            magnetic.SetActive(active);
        }

        public void SetCollectCoin()
        {
            _currentCoinCollect++;
            if (_currentCoinCollect > 0)
            {
                magnetEffect.SetActive(true);
            }
        }

        public void CollectComplete()
        {
            _currentCoinCollect--;
            if (_currentCoinCollect <= 0)
            {
                _currentCoinCollect = 0;
                magnetEffect.SetActive(false);
            }
        }
        
    }
}