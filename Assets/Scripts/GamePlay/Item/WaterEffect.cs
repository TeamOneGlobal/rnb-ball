using UnityEngine;

namespace GamePlay.Item
{
    [ExecuteAlways]
    public class WaterEffect : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer,iceRenderer;
        [SerializeField] private new BoxCollider2D collider2D;
        [SerializeField] private BuoyancyEffector2D effect;
        [SerializeField] private Vector2 deltaCollider;
        [SerializeField] private float deltaSurface = 2f;
        private void Update()
        {
            var size = renderer.size;
            collider2D.size = size - deltaCollider;
            collider2D.offset = -deltaCollider/2;
            effect.surfaceLevel = size.y / 2-deltaSurface - deltaCollider.y;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
    }
}