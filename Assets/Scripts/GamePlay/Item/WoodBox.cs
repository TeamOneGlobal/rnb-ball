using Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Item
{
    public class WoodBox : ObjectCollision
    {
        [SerializeField] private LayerMask groundAndPlatformLayers;
        [SerializeField] private SimpleAudio simpleAudio;
        [SerializeField] private AudioClip move, drop;

        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private bool grounded;
        [SerializeField] private Vector2 baseForce;
        private bool _isMove, _isDrop;
        private bool _isCollisionBall;
        private void Update()
        {
            var velocity = rigidbody.velocity;
            grounded = CheckIsGrounded();
            if (Mathf.Abs(velocity.x) > 0.5f && grounded)
            {
                if (!_isMove )
                {
                    _isMove = true;
                    simpleAudio.Play(move, true).Forget();
                }
               
            }
            else
            {
                if (_isMove)
                {
                    _isMove = false;
                    simpleAudio.Stop();
                }
            }
            if (velocity.y < 0 && !_isDrop&& !grounded)
            {
                _isDrop = true;
            }
            if (_isDrop && grounded )
            {
                simpleAudio.Play(drop).Forget();
                _isDrop = false;
            }
            if(!_isCollisionBall) return;
            if(GamePlayController.Instance.controlCharacter.transform.position.y-transform.position.y<1f) return;
            if(!GamePlayController.Instance.controlCharacter.CollisionWithGround()) return;
            var charVelocity = GamePlayController.Instance.controlCharacter.GetVelocity();
            //if(Mathf.Abs(charVelocity.y)>1f) return;
            if (charVelocity.x<0)
            {
                rigidbody.velocity = baseForce;
            }
            else if (charVelocity.x>0)
            {
                rigidbody.velocity = new Vector2(-baseForce.x,baseForce.y);
            }
        }

        protected override void CollisionEnter(string triggerTag, Transform triggerObject)
        {
            base.CollisionEnter(triggerTag, triggerObject);
            _isCollisionBall = true;
        }

        protected override void CollisionExit(string triggerTag, Transform triggerObject)
        {
            base.CollisionExit(triggerTag, triggerObject);
            _isCollisionBall = false;
        }

        private bool CheckIsGrounded()
        {
            Vector2 boundSize = gameObject.GetComponent<Collider2D>().bounds.size;
            boundSize = new Vector2(boundSize.x - 0.1f, boundSize.y);
            RaycastHit2D hit2D = Physics2D.BoxCast(gameObject.GetComponent<Collider2D>().bounds.center, boundSize,
                0, Vector2.down, 0.1f, groundAndPlatformLayers);

            return hit2D.collider != null;
        }
    }
}