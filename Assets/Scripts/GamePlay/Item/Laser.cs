using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using GamePlay.Item;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

[ExecuteAlways]
public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private float textureScrollSpeed;
    [SerializeField] private float textureLengthScale = 0.01f; 
    [SerializeField] private Transform startPoint;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform end;
    [SerializeField] private CheckPoint checkPoint;
    private float _distance;
    [SerializeField] private float _maxDistance = 10000f;
    private List<Vector3> _laserPoints;
    [SerializeField] private Vector2 _direction;
    private void Start()
    {
        _laserPoints = new List<Vector3>();
    }

    public void SetRange(float range)
    {
        _maxDistance = range;
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
        _laserPoints= GetLaserPoint();
        laserLine.positionCount = _laserPoints.Count();
        laserLine.SetPositions(_laserPoints.ToArray());
        end.position = _laserPoints.Last();
        _distance = Vector3.Distance(laserLine.GetPosition(1), laserLine.GetPosition(0));
        
        var sharedMaterial = laserLine.sharedMaterial;
        sharedMaterial.mainTextureScale = new Vector2(_distance / textureLengthScale, 1);
        sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }

    private List<Vector3> GetLaserPoint()
    {
        var result = new List<Vector3>();
        var hit2Ds = new RaycastHit2D[1];
        result.Add(startPoint.position);
        Physics2D.RaycastNonAlloc(startPoint.position, _direction, hit2Ds, _maxDistance,layerMask);
        if (hit2Ds[0].collider != null)
        {
            var controller = hit2Ds[0].collider.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.Damage(checkPoint,Vector2.zero);
            }

            result.Add(new Vector3(hit2Ds[0].point.x,hit2Ds[0].point.y,100));
        }
        else
        {
            var max = startPoint.position+ (Vector3)_direction * _maxDistance;
            result.Add(new Vector3(max.x,max.y,100));
        }

        return result;
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