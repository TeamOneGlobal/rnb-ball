using System;
using UnityEngine;

namespace GamePlay.Item
{
    public class WaterEffect : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer,iceRenderer;
        [SerializeField] private new BoxCollider2D collider2D;
        [SerializeField] private BuoyancyEffector2D effect;
        [SerializeField] private Vector2 deltaCollider;
        [SerializeField] private float deltaSurface = 2f;
        [SerializeField] private float speed = 2f, delta = 0;
        private MaterialPropertyBlock _propBlock;
        private void Awake()
        {
            _propBlock =  new MaterialPropertyBlock();
        }

        private void Update()
        {
            var size = renderer.size;
            collider2D.size = size - deltaCollider;
            collider2D.offset = -deltaCollider/2;
            effect.surfaceLevel = size.y / 2-deltaSurface - deltaCollider.y;

            delta += Time.deltaTime * speed;
            renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetVector("_MainTex_ST",new Vector4(1f, 1f, delta, 0));
            renderer.SetPropertyBlock(_propBlock);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var size = renderer.size;
                collider2D.size = size - deltaCollider;
                collider2D.offset = -deltaCollider/2;
                effect.surfaceLevel = size.y / 2-deltaSurface - deltaCollider.y;
            }
#endif
        }
    }
}