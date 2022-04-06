using System;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using Truongtv.Utilities;
using UnityEngine;
using UpdateType = Com.LuisPedroFonseca.ProCamera2D.UpdateType;

namespace GamePlay.CameraControl
{
    public class CinematicView : SingletonMonoBehavior<CinematicView>
    {
        [SerializeField] private ProCamera2DNumericBoundaries boundaries;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float moveTime, waitTime, moveBackTime;
        [SerializeField] private float changeTargetTime = 0.5f;
        private ProCamera2D _camera2D;
        private Vector2 _canvasSize;
        private void Start()
        {
            _camera2D = ProCamera2D.Instance;
            _canvasSize = canvas.renderingDisplaySize;
        }

        public void StartCinematic(Transform secondObj,Action complete = null)
        {
            _camera2D.UpdateType = UpdateType.ManualUpdate;
            var nextPosition = secondObj.position;
            _camera2D.transform.DOMove(nextPosition, moveTime)
                .SetEase(Ease.Linear)
                .SetUpdate(DG.Tweening.UpdateType.Normal,true)
                .OnUpdate(() =>
                {
                    var transformPosition = _camera2D.transform.position;

                    if (boundaries.UseLeftBoundary &&
                        transformPosition.x - _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y <
                        boundaries.LeftBoundary)
                    {
                        transformPosition.x = boundaries.LeftBoundary +
                                         _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y;
                    }

                    if (boundaries.UseRightBoundary &&
                        transformPosition.x + _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y >
                        boundaries.RightBoundary)
                    {
                        transformPosition.x = boundaries.RightBoundary -
                                         _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y;
                    }

                    if (boundaries.UseTopBoundary &&
                        transformPosition.y + _camera2D.GameCamera.orthographicSize > boundaries.TopBoundary)
                    {
                        transformPosition.y = boundaries.TopBoundary - _camera2D.GameCamera.orthographicSize;
                    }

                    if (boundaries.UseBottomBoundary && transformPosition.y - _camera2D.GameCamera.orthographicSize <
                        boundaries.BottomBoundary)
                    {
                        transformPosition.y = boundaries.BottomBoundary + _camera2D.GameCamera.orthographicSize;
                    }

                    transformPosition.z = -10;
                    _camera2D.transform.position = transformPosition;
                    
                }).OnComplete(() =>
                {
                    complete?.Invoke();
                });
        }

        public void MoveBack(Transform firstObj,Action complete = null)
        {
            _camera2D.UpdateType = UpdateType.ManualUpdate;
            var nextPosition = firstObj.position;
            _camera2D.transform.DOMove(nextPosition, moveBackTime)
                .SetDelay(waitTime)
                .SetEase(Ease.Linear)
                .SetUpdate(DG.Tweening.UpdateType.Normal,true)
                .OnUpdate(() =>
                {
                    UpdateCameraPosition(_camera2D.transform.position);
                })
                .OnComplete(() =>
                {
                    _camera2D.UpdateType = UpdateType.FixedUpdate;
                    complete?.Invoke();
                    GamePlayController.Instance.PauseForCinematic(false);
                });
        }
        public void ChangeTarget(Transform newTarget)
        {
            _camera2D.UpdateType = UpdateType.ManualUpdate;
            _camera2D.transform.DOMove(newTarget.position, changeTargetTime)
                .SetEase(Ease.Linear)
                .SetUpdate(DG.Tweening.UpdateType.Normal,true)
                .OnUpdate(() =>
                {
                    UpdateCameraPosition(_camera2D.transform.position);
                })
                .OnComplete(() =>
                {
                    _camera2D.CameraTargets[0].TargetTransform = newTarget;
                    _camera2D.CenterOnTargets();
                    _camera2D.UpdateType = UpdateType.FixedUpdate;
                });
        }

        public void UpdateCameraPosition(Vector3 transformPosition)
        {
            if (boundaries.UseLeftBoundary &&
                transformPosition.x - _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y <
                boundaries.LeftBoundary)
            {
                transformPosition.x = boundaries.LeftBoundary +
                                      _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y;
            }

            if (boundaries.UseRightBoundary &&
                transformPosition.x + _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y >
                boundaries.RightBoundary)
            {
                transformPosition.x = boundaries.RightBoundary -
                                      _camera2D.GameCamera.orthographicSize * _canvasSize.x / _canvasSize.y;
            }

            if (boundaries.UseTopBoundary &&
                transformPosition.y + _camera2D.GameCamera.orthographicSize > boundaries.TopBoundary)
            {
                transformPosition.y = boundaries.TopBoundary - _camera2D.GameCamera.orthographicSize;
            }

            if (boundaries.UseBottomBoundary && transformPosition.y - _camera2D.GameCamera.orthographicSize <
                boundaries.BottomBoundary)
            {
                transformPosition.y = boundaries.BottomBoundary + _camera2D.GameCamera.orthographicSize;
            }

            transformPosition.z = -10;
            _camera2D.transform.position = transformPosition;
        }
    }
}