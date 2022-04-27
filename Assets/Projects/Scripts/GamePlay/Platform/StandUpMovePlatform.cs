using DG.Tweening;

namespace GamePlay.Platform
{
    public class StandUpMovePlatform : BaseMovingPlatform
    {
        private void Start()
        {
            platform.onCharacterEnter += () =>
            {
                platform.transform.DOKill();
                Move();
            };
            platform.onCharacterExit += () =>
            {
                platform.transform.DOKill();
                ReserveMove();
            };
        }
    }
}