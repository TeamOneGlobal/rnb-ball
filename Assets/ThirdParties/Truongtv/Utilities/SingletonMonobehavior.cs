using Sirenix.OdinInspector;
using UnityEngine;

namespace Truongtv.Utilities
{
    public class SingletonMonoBehavior<T> : SerializedMonoBehaviour where T : Component
    {
        private static T _instance;

        [HideInInspector]public static T Instance => _instance;

        public virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            else Destroy(gameObject);
        }

       
    }
}