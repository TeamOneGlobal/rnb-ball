using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Item
{
    public class Mirror : MonoBehaviour
    {
        
        public Vector2 Reflex(Vector2 inputDirection)
        {
            return Vector2.Reflect(inputDirection, transform.up);
        }
    }
}