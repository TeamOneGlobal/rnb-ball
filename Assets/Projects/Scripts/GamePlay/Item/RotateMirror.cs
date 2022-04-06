using System;
using UnityEngine;

namespace GamePlay.Item
{
    public class RotateMirror : MonoBehaviour
    {
        [SerializeField] private Transform mover, mirror;


        private void Update()
        {
            var x = mover.localPosition.x;
            var angle = x / 2 * 180;
            mirror.transform.localEulerAngles = new Vector3(0,0,angle);
        }
    }
}