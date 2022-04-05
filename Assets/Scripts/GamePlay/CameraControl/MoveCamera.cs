using System;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using Truongtv.Utilities;
using UnityEngine;
using UpdateType = Com.LuisPedroFonseca.ProCamera2D.UpdateType;

namespace GamePlay.CameraControl
{
    public class MoveCamera : SingletonMonoBehavior<MoveCamera>
    {
        [SerializeField] private float zoomOnMove = 12;
        [SerializeField] private float cameraSpeed = 10;
        [SerializeField] private ProCamera2DNumericBoundaries boundaries;
        [SerializeField] private Canvas canvas;
        private ProCamera2D _camera2D;
        private float _baseCameraView;
        private Vector2 _canvasSize; 
        private Vector2 _input;
        private bool _onDrag;
        [HideInInspector]public  Action onStartMove, onEndMove;
        private void Start()
        {
            _camera2D = ProCamera2D.Instance;
            _baseCameraView = _camera2D.GameCamera.orthographicSize;
            _canvasSize = canvas.renderingDisplaySize;
        }


        public void OnPointerDown()
        {
            onStartMove ?.Invoke();
            _onDrag = true;
            DOTween.To(()=> _camera2D.GameCamera.orthographicSize, x=> _camera2D.GameCamera.orthographicSize = x, zoomOnMove, 0.5f).SetEase(Ease.Linear);
            _camera2D.UpdateType = UpdateType.ManualUpdate;
        }

        public void OnPointerUp()
        {
            DOTween.To(()=> _camera2D.GameCamera.orthographicSize, x=> _camera2D.GameCamera.orthographicSize = x, _baseCameraView, 0.5f).SetEase(Ease.Linear);
            var nextPosition = _camera2D.CameraTargetPosition;
            
            nextPosition.z = -10;
            nextPosition.x += _camera2D.OffsetX;
            nextPosition.y += _camera2D.OffsetY;
           var tween =  _camera2D.transform.DOMove(nextPosition,0.75f).SetEase(Ease.Linear);
          
           tween.OnUpdate(() =>
           {
               var nextPosition = _camera2D.transform.position ;

               if (boundaries.UseLeftBoundary && nextPosition.x-_camera2D.GameCamera.orthographicSize*_canvasSize.x/_canvasSize.y < boundaries.LeftBoundary)
               {
                   nextPosition.x = boundaries.LeftBoundary + _camera2D.GameCamera.orthographicSize*_canvasSize.x/_canvasSize.y;
               }
               if (boundaries.UseRightBoundary && nextPosition.x+_camera2D.GameCamera.orthographicSize*_canvasSize.x/_canvasSize.y > boundaries.RightBoundary)
               {
                   nextPosition.x = boundaries.RightBoundary - _camera2D.GameCamera.orthographicSize *_canvasSize.x/_canvasSize.y;
               }
               if (boundaries.UseTopBoundary && nextPosition.y+_camera2D.GameCamera.orthographicSize > boundaries.TopBoundary)
               {
                   nextPosition.y = boundaries.TopBoundary - _camera2D.GameCamera.orthographicSize ;
               }
               if (boundaries.UseBottomBoundary && nextPosition.y-_camera2D.GameCamera.orthographicSize < boundaries.BottomBoundary)
               {
                   nextPosition.y = boundaries.BottomBoundary + _camera2D.GameCamera.orthographicSize;
               }

               nextPosition.z = -10;
               _camera2D.transform.position = nextPosition;
           });
            _onDrag = false;
            _camera2D.UpdateType = UpdateType.FixedUpdate;
            onEndMove?.Invoke();
        }

        public void OnDrag(Vector2 value)
        {
            _input = value;
        }

        private void Update()
        {
            if(!_onDrag) return;
            var nextPosition = _camera2D.transform.position += (Vector3) _input * cameraSpeed;

            if (boundaries.UseLeftBoundary && nextPosition.x-_camera2D.GameCamera.orthographicSize*_canvasSize.x/_canvasSize.y < boundaries.LeftBoundary)
            {
                nextPosition.x = boundaries.LeftBoundary + _camera2D.GameCamera.orthographicSize*_canvasSize.x/_canvasSize.y;
            }
            if (boundaries.UseRightBoundary && nextPosition.x+_camera2D.GameCamera.orthographicSize*_canvasSize.x/_canvasSize.y > boundaries.RightBoundary)
            {
                nextPosition.x = boundaries.RightBoundary - _camera2D.GameCamera.orthographicSize *_canvasSize.x/_canvasSize.y;
            }
            if (boundaries.UseTopBoundary && nextPosition.y+_camera2D.GameCamera.orthographicSize > boundaries.TopBoundary)
            {
                nextPosition.y = boundaries.TopBoundary - _camera2D.GameCamera.orthographicSize ;
            }
            if (boundaries.UseBottomBoundary && nextPosition.y-_camera2D.GameCamera.orthographicSize < boundaries.BottomBoundary)
            {
                nextPosition.y = boundaries.BottomBoundary + _camera2D.GameCamera.orthographicSize;
            }

            nextPosition.z = -10;
            _camera2D.transform.position = nextPosition;
        }
    }
}