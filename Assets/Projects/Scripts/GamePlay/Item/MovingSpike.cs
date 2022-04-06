using DG.Tweening;
using Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Item
{
    public class MovingSpike : MonoBehaviour
    {
        [SerializeField] private Transform spike;
        [SerializeField] private Transform start, end;
        [SerializeField] private float timeUp, timeDown;
        [SerializeField] private float delayUp, delayDown;
        [SerializeField] private SimpleAudio simpleAudio;
        private Vector3 _startPos, _endPos;
        private void Start()
        {
            _startPos = start.position;
            _endPos = end.position;
            spike.position = _startPos;
            MoveUp();
        }

        private void MoveUp()
        {
            spike.DOMoveY(_endPos.y, timeUp).SetDelay(delayUp).SetEase(Ease.InExpo).OnComplete(()=>
            {
                simpleAudio.Play().Forget();
                MoveDown();
            });
        }

        private void MoveDown()
        {
            spike.DOMoveY(_startPos.y, timeDown).SetDelay(delayDown).SetEase(Ease.InExpo).OnComplete(MoveUp);
        }

        private void OnDestroy()
        {
            spike.DOKill();
        }
    }
}