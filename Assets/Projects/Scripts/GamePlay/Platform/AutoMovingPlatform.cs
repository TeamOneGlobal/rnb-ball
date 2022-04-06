namespace GamePlay.Platform
{
    public class AutoMovingPlatform : BaseMovingPlatform
    {
       
        private void Start()
        {
            CurrentPoint = 0;
            //PlaySoundMoving();
            AutoMove();
        }

        private void AutoMove()
        {
            Move(false,AutoReserveMove);
        }

        private void AutoReserveMove()
        {
            ReserveMove(false,AutoMove);
        }
    }
} 