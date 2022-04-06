using UnityEngine;

namespace GamePlay.Characters
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private CharacterMovement target;
        private void Update()
        {
            if (target.moveDirection == MoveDirection.Left)
            {
                transform.localScale = new Vector3(-1,1,1);
            }
            else  if (target.moveDirection == MoveDirection.Right)
            {
                transform.localScale = new Vector3(1,1,1);
            }
            transform.eulerAngles = Vector3.zero;
        }
    }
}