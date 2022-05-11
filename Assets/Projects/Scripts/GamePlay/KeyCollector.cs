using System;
using DG.Tweening;
using Truongtv.Utilities;
using UnityEngine;

namespace GamePlay
{
    public class KeyCollector : SingletonMonoBehavior<KeyCollector>
    {
        [SerializeField] public GameObject blueKey, redKey;
        
        private bool _isBlueKeyCollected, _isRedKeyCollected;
        public Action collectRed, collectBlue;
        private void Start()
        {
            blueKey.SetActive(false);
            redKey.SetActive(false);
        }
        public void CollectKey(string collisionTag, Vector3 collectPosition)
        {
            var sequence = DOTween.Sequence();
            if (collisionTag.Equals(TagManager.BLUE_TAG))
            {
                
                blueKey.transform.position = new Vector3(collectPosition.x,collectPosition.y,blueKey.transform.position.z);
                blueKey.SetActive(true);
                
                sequence.Append(blueKey.transform.DOLocalMoveX(0,0.75f).SetEase(Ease.OutBack));
                sequence.Join(blueKey.transform.DOLocalMoveY(0,0.75f).SetEase(Ease.Linear));
                _isBlueKeyCollected = true;
                collectBlue?.Invoke();
            }
            else if (collisionTag.Equals(TagManager.RED_TAG))
            {
                redKey.transform.position = new Vector3(collectPosition.x,collectPosition.y,redKey.transform.position.z);;
                redKey.SetActive(true);
                _isRedKeyCollected = true;
                sequence.Append(redKey.transform.DOLocalMoveX(0,0.75f).SetEase(Ease.OutBack));
                sequence.Join(redKey.transform.DOLocalMoveY(0,0.75f).SetEase(Ease.Linear));
                _isRedKeyCollected = true;
                collectRed?.Invoke();
            }
            sequence.Play();
        }

        public bool IsKeyCollected(string collisionTag)
        {
            if (collisionTag.Equals(TagManager.BLUE_TAG))
                return _isBlueKeyCollected;
            if (collisionTag.Equals(TagManager.RED_TAG))
                return _isRedKeyCollected;
            return false;
        }
    }
}