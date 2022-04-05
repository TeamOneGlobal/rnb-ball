using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class ToggleHelper : MonoBehaviour
    {
        [SerializeField] private Graphic onGroup, offGroup;
        [SerializeField] private Toggle toggle;


        private void Awake()
        {
            toggle.onValueChanged.AddListener(Change);
            Change(toggle.isOn);
        }

        private void Change(bool isOn)
        {
            onGroup.CrossFadeAlpha(isOn ? 1f : 0f,  0.1f, true);
            offGroup.CrossFadeAlpha(isOn ? 0f : 1f,  0.1f, true);
            
            // onGroup.SetActive(isOn);
            // offGroup.SetActive(!isOn);
        }
    }
}