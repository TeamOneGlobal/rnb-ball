using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GamePlay
{
    [RequireComponent(typeof(Collider2D))]
    public class ObjectCollision : MonoBehaviour
    {
        [SerializeField,ValueDropdown(nameof(GetAllTriggerTag))] protected List<string> collisionTags;
        [SerializeField] private bool useDirectly;

        [SerializeField, ShowIf(nameof(useDirectly))]
        private UnityEvent<string,Transform> OnEnter,OnStay,OnExit;
        protected Collider2D Collider2D;

        private void Awake()
        {
            Collider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collisionTags.Contains(collision.gameObject.tag)) {
                CollisionEnter(collision.gameObject.tag,collision.transform);
                if(useDirectly)
                    OnEnter.Invoke(collision.gameObject.tag,collision.transform);
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collisionTags.Contains(collision.gameObject.tag)) {
                CollisionExit(collision.gameObject.tag,collision.transform);
                if(useDirectly)
                    OnEnter.Invoke(collision.gameObject.tag,collision.transform);
            }
        }

        protected virtual void CollisionEnter(string triggerTag,Transform triggerObject) { 
        }
        protected virtual void CollisionExit(string triggerTag,Transform triggerObject) { 
        }

        private string[] GetAllTriggerTag()
        {
            return TagManager.GetAllTagHandle();
        }

    }
}