using UnityEngine;

namespace GamePlay.Characters
{
    public class CharacterSelected : MonoBehaviour
    {
        [SerializeField] private GameObject selected;
        [SerializeField] private GameObject particle;
        [SerializeField] private new AudioListener audio;
        private CharacterController _controller;
        public void Init(CharacterController controller)
        {
            _controller = controller;
        }
        public void OnCharacterSelected()
        {
            selected.SetActive(true);
            particle.SetActive(true);
            audio.enabled = true;
        }

        public void OnCharacterUnSelected()
        {
            selected.SetActive(false);
            particle.SetActive(false);
            audio.enabled = false;
        }

        public void HideBall(bool isHide)
        {
            selected.SetActive(!isHide);
        }
    }
}