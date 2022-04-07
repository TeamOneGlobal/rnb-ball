using System;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleHelper : MonoBehaviour
    {
        [SerializeField] protected Graphic[] onObjects,offObjects;
        [SerializeField] protected Toggle _toggle;
        protected virtual void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(value=>
            {
                OnValueChange(value,0.1f);
            });
            _toggle.onValueChanged.Invoke(_toggle.isOn);
        }

        protected void OnValueChange(bool value,float duration)
        {
            
            if (onObjects?.Length>0)
            {
                foreach (var obj in onObjects)
                {
                    if (duration == 0)
                    {
                        var color = obj.color;
                        color.a = value ? 1f : 0f;
                        obj.color = color;
                    }
                    else
                    {
                        obj.CrossFadeAlpha(value ? 1f : 0f,  duration, true);
                    }
                }
            }
            if (offObjects?.Length>0)
            {
                foreach (var obj in offObjects)
                {
                    if (duration == 0)
                    {
                        var color = obj.color;
                        color.a = value ? 0f:1f;
                        obj.color = color;
                    }
                    else
                        obj.CrossFadeAlpha(value ? 0f:1f,  duration, true);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying) return;
            _toggle = GetComponent<Toggle>();
            if (onObjects?.Length>0)
            {
                foreach (var obj in onObjects)
                {
                    obj.CrossFadeAlpha(_toggle.isOn ? 1f : 0f,  0.1f, true);
                }
            }
            if (offObjects?.Length>0)
            {
                foreach (var obj in offObjects)
                {
                    obj.CrossFadeAlpha(_toggle.isOn ? 0f:1f,  0.1f, true);
                }
            }
        }
    }

}