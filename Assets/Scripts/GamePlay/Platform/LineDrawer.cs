using UnityEngine;

namespace GamePlay.Platform
{
    [ExecuteAlways]
    public class LineDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform[] points;
        private Vector3[] _pos;

        private void Awake()
        {
            _pos = new Vector3[points.Length];
            InitPoint();
            lineRenderer.SetPositions(_pos);
            lineRenderer.useWorldSpace = true;
        }

        private void Update()
        {
            InitPoint();
            lineRenderer.SetPositions(_pos);
        }

        private void InitPoint()
        {
            for (var i = 0; i < points.Length; i++)
            {
                _pos[i] = points[i].position;
            }
        }

        void OnDrawGizmos()
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