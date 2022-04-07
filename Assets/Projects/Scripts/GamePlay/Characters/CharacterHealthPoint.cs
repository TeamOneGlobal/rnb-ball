using System;
using Cysharp.Threading.Tasks;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Characters
{
    public class CharacterHealthPoint : MonoBehaviour
    {
        [SerializeField] private int maxHp = 1;
        [SerializeField] private GameObject dieEffect;
        [SerializeField] private AudioClip[] diesSounds, reviveSounds;
        [SerializeField] private new SimpleAudio audio;
        private int _currentHp;
        private CheckPoint _reviveCheckPoint;
        private bool _isDie;
        private CharacterController _controller;
        public void Init(CharacterController controller)
        {
            _controller = controller;
        }
        public void Revive(Action onComplete = null)
        {
            _isDie = false;
            _reviveCheckPoint.StartSpawnEffect(() =>
            {
                _controller.HideBall(false);
                var r = Random.Range(0, reviveSounds.Length);
                audio.Play(reviveSounds[r]);
                _controller.PlayIdle();
                _controller.CancelAllMove();
                _currentHp = maxHp;
                transform.position = _reviveCheckPoint.transform.position;
            },onComplete);
        }
        public void Damage( CheckPoint checkPoint,int damage = 1)
        {
            if(_isDie) return;
            _reviveCheckPoint = checkPoint;
            _currentHp -= damage;
            if (_currentHp > 0) return;
            if(!_isDie)
                Die();

        }
        private void Die()
        {
            _isDie = true;
            var r = Random.Range(0, diesSounds.Length);
            audio.Play(diesSounds[r]);
            _controller.PlayDie(async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                dieEffect.SetActive(true);
                _controller.HideBall(true);
            });
            GamePlayController.Instance.CharacterDie();
        }

        public bool IsDie()
        {
            return _isDie;
        }
    }
}