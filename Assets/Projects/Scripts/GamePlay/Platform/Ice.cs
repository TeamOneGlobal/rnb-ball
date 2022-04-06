using DG.Tweening;
using UnityEngine;

namespace GamePlay.Platform
{
    [ExecuteAlways]
    public class Ice : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer iceRenderer;
        [SerializeField] private BoxCollider2D collider2D;
        private void Update()
        {
            var size = iceRenderer.size;
            collider2D.size = size;
            collider2D.offset = Vector2.zero;
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

        public void OnActive()
        {
            float alpha = 0;
            DOTween.To(()=> iceRenderer.color.a, setter: x => alpha = x, 1,0.5f)
                .OnUpdate(() =>
                {
                    var color = iceRenderer.color;
                    color.a = alpha;
                    iceRenderer.color = color;
                })
                .OnComplete(() =>
                {
                    collider2D.isTrigger = false;
                });
        }

        public void DeActive()
        {
            var alpha = 1f;
            DOTween.To(()=> iceRenderer.color.a, x => alpha = x, 0,0.5f)
                .OnUpdate(() =>
                {
                    var color = iceRenderer.color;
                    color.a = alpha;
                    iceRenderer.color = color;
                });
            collider2D.isTrigger = true;
        }
    }
}