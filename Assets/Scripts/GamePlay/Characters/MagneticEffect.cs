using UnityEngine;

namespace GamePlay.Characters
{
    public class MagneticEffect : MonoBehaviour
    {
        [SerializeField] private CharacterMagnetic magnet;

        public void SetCollectCoin()
        {
            magnet.SetCollectCoin();
        }

        public void SetCollectComplete()
        {
            magnet.CollectComplete();
        }
    }
}