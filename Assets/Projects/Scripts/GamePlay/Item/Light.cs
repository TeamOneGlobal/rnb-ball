using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Item
{
    // [ExecuteAlways]
    public class Light : MonoBehaviour
    {
        [SerializeField] private bool active = true;
        [SerializeField] private LineRenderer laserLine;
        [SerializeField] private Transform startPoint,endPoint;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float delta;
        private float _distance;
        [SerializeField] private float maxDistance = 10000f;
        private List<Vector3> _laserPoints;
        private Vector2 _direction;

        private void Start()
        {
            _laserPoints = new List<Vector3>();
            AutoUpdateDirection();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void AutoUpdateDirection()
        {
            _direction = startPoint.up;
        }

        void Update()
        {
            if(!active) return;
            _laserPoints = GetLaserPoint();
            laserLine.positionCount = _laserPoints.Count();
            laserLine.SetPositions(_laserPoints.ToArray());
            endPoint.transform.position = _laserPoints.Last();
            //_distance = Vector3.Distance(laserLine.GetPosition(1), laserLine.GetPosition(0));

//        var sharedMaterial = laserLine.sharedMaterial;
//        sharedMaterial.mainTextureScale = new Vector2(_distance / textureLengthScale, 1);
//        sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
        }

        private List<Vector3> GetLaserPoint()
        {
            var result = new List<Vector3>();
            var startPosition = startPoint.position;
            result.Add(startPosition);
            var direction = _direction;
            startPosition.z = 100;
            Vector3 newPosition;
            
            bool find = true;
            while (find)
            {
                var raycastHit2D = Physics2D.Raycast(startPosition, direction, maxDistance, layerMask);
                if (raycastHit2D.collider == null)
                {
                    var max = startPosition + (Vector3) direction * maxDistance;
                    result.Add(new Vector3(max.x, max.y, 100));
                    break;
                }
                newPosition = new Vector3(raycastHit2D.point.x, raycastHit2D.point.y, 100);
                result.Add(newPosition);
                var mirror = raycastHit2D.collider.GetComponent<Mirror>();
                if (mirror != null)
                {
                    startPosition = newPosition - (Vector3) direction * delta;
                    startPosition.z = 100;
                    direction = mirror.Reflex(direction);
                }
                else
                {
                    var lightSource = raycastHit2D.collider.GetComponent<LightReceiver>();
                    if (lightSource != null)
                    {
                        lightSource.LightTrigger();
                    }
                    break;
                }
            }

            return result;
        }

        public void Active(bool activeLight)
        {
            active = activeLight;
        }

//    void OnDrawGizmos()
//    {
//#if UNITY_EDITOR
//        if (!Application.isPlaying)
//        {
//            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
//            UnityEditor.SceneView.RepaintAll();
//        }
//#endif
//    }
    }
}