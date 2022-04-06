using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Item
{
    public class DamageObject: ObjectTrigger
    {
        [SerializeField] private CheckPoint checkPoint;
        [SerializeField] private Vector2 addForce;
        protected void DamageTarget(Transform target)
        {
            target.GetComponent<CharacterController>().Damage(checkPoint,addForce);
        }
        
    }
}