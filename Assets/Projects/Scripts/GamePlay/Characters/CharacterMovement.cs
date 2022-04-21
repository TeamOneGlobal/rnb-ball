using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private Transform icon;
        [SerializeField] public MoveDirection moveDirection = MoveDirection.None;
        [SerializeField] private LayerMask groundAndPlatformLayers;
        [Title("MOVE CONFIG")] [SerializeField]
        private float moveMaxSpeed = 10f;
        [SerializeField] private float moveSpeedIncrease = 30f;
        [SerializeField] private float moveSpeedDecrease = 30f;
        [SerializeField] private float deccelTime = 1f;
        [Title("JUMP CONFIG")] [SerializeField]
        private float jumpSpeed = 22f;

        [SerializeField] private float jumpSpeedUp = 35f;
        [SerializeField] private float jumpSpeedDown = 40f;

        [SerializeField, MinMaxSlider(-100, 100, true)]
        private Vector2 jumpSpeedRange;

        [Title("Effect")] 
        [SerializeField] private GameObject landedEffect;
        [SerializeField] private GameObject moveLeftEffect,moveRightEffect;
        [Title("MOVE Sound")] [SerializeField] private SimpleAudio simpleAudio;

        [SerializeField] private AudioClip jumpSound, landedSound;
        private Vector2 _velocity;
        private bool _isGrounded,_isWater;
        private MoveDirection _moveEffect = MoveDirection.None;
        private CharacterController _controller;
        private float countDeccelTime;
        public void Init(CharacterController controller)
        {
            _controller = controller;
            //moveMaxSpeed = Config.BALL_MAX_MOVE_SPEED;
        }
        private void Update()
        {
            UpdateJumpState();
            ComputeVelocity();
            RollEffect();
        }

        private void UpdateJumpState()
        {
            var isGround = CheckIsGrounded();
            if (isGround && !_isGrounded && _velocity.y<-10f && !_isWater)
            {
                EffectJump();
            }
            _isGrounded = isGround;
        }

        private void ComputeVelocity()
        {
            _velocity = rigidbody2D.velocity;
            if (moveDirection != MoveDirection.None)
            {
                countDeccelTime = deccelTime;
                if (_velocity.x * (int) moveDirection < moveMaxSpeed)
                {
                    _velocity.x +=  (int) moveDirection*moveSpeedIncrease*Time.deltaTime;
                       
                }
            }
            else
            {
                countDeccelTime -= Time.deltaTime;
                if (countDeccelTime > 0)
                {
                    if (_velocity.x < 0)
                    {
                        _velocity.x += moveSpeedDecrease * Time.deltaTime;
                        if (_velocity.x > 0) _velocity.x = 0;
                    }
                    else if (_velocity.x > 0)
                    {
                        _velocity.x -= moveSpeedDecrease * Time.deltaTime;
                        if (_velocity.x < 0) _velocity.x = 0;
                    }
                }
            }

            if (!_isGrounded)
            {
                
            }
            if (_velocity.y > 0)
            {
                _velocity.y += -jumpSpeedUp * Time.deltaTime;
            }
            else
            {
                _velocity.y += -jumpSpeedDown * Time.deltaTime;
            }

            if (_velocity.y > jumpSpeedRange.y) _velocity.y = jumpSpeedRange.y;
            if (_velocity.y < jumpSpeedRange.x) _velocity.y = jumpSpeedRange.x;
            rigidbody2D.velocity = _velocity;
            
        }

        #region move

        public async UniTaskVoid Jump()
        {
            if (!_isGrounded&&!_isWater) return;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);
            simpleAudio.Play(jumpSound);
            await UniTask.WaitUntil(() => _isGrounded);
            _velocity.y = 0;
        }

        public void MoveLeft()
        {
            moveDirection = MoveDirection.Left;
        }

        public void EndMoveLeft()
        {
            if (moveDirection != MoveDirection.Right)
                moveDirection = MoveDirection.None;
        }

        public void EndMoveRight()
        {
            if (moveDirection != MoveDirection.Left)
                moveDirection = MoveDirection.None;
        }

        public void MoveRight()
        {
            moveDirection = MoveDirection.Right;
        }

        public bool IsJumping()
        {
            return !_isGrounded && !_isWater;
        }
        private bool CheckIsGrounded()
        {
            Vector2 boundSize = gameObject.GetComponent<CircleCollider2D>().bounds.size;
            boundSize = new Vector2(0.1f, boundSize.y);
            RaycastHit2D hit2D = Physics2D.BoxCast(gameObject.GetComponent<CircleCollider2D>().bounds.center, boundSize,
                180, Vector2.down, 0.1f, groundAndPlatformLayers);

            return hit2D.collider != null;
        }
        public void AddForce(Vector2 force)
        {
            rigidbody2D.velocity += force;
        }
        public Vector2 GetVelocity()
        {
            return _velocity;
        }
        public void SetOnWater(bool value)
        {
            _isWater = value;
        }
        #endregion
        #region Effect


        private void EffectJump()
        {
            var eff = Instantiate(landedEffect);
            eff.transform.position = new Vector3(0,-0.65f,0)+transform.position;
            simpleAudio.Play(landedSound);
        }

        private MoveDirection _roll;
        private void RollEffect()
        {
            if (moveDirection == MoveDirection.Left)
            {
                if (_roll != MoveDirection.Left)
                {
                    _roll = MoveDirection.Left;
                    icon.DOLocalRotate(new Vector3(0,0,180f), 1.75f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
                }
            }
            else if (moveDirection == MoveDirection.Right)
            {
                if (_roll != MoveDirection.Right)
                {
                    _roll = MoveDirection.Right;
                    icon.DOLocalRotate(new Vector3(0,0,-180f), 1.75f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
                }
            }
            else
            {
                _roll = MoveDirection.None;
                icon.DOKill();
            }

            if (Mathf.Abs(_velocity.x) < 3f)
            {
                _moveEffect = MoveDirection.None;
            }

            if (_velocity.x >= 3f && _moveEffect == MoveDirection.None && moveDirection == MoveDirection.Right)
            {
                _moveEffect = MoveDirection.Right;
                if(_isGrounded && !_isWater)
                {
                    MoveEffect(_moveEffect);
                }
            }

            if (_velocity.x <= -3f && _moveEffect == MoveDirection.None && moveDirection == MoveDirection.Left)
            {
                _moveEffect = MoveDirection.Left;
                if(_isGrounded && !_isWater)
                {
                    MoveEffect(_moveEffect);
                }
            }

            
        }

        private void MoveEffect(MoveDirection direction)
        {
            GameObject eff;
            if (direction == MoveDirection.Left)
            {
                eff = Instantiate(moveLeftEffect);
                eff.transform.position = new Vector3(0.5f,-0.65f,0)+transform.position;
            }

            
            else if (direction == MoveDirection.Right)
            {
                eff = Instantiate(moveRightEffect);
                eff.transform.position = new Vector3(-0.5f,-0.65f,0)+transform.position;
            }
        }
        #endregion

        public void HideBall(bool isHide)
        {
            icon.gameObject.SetActive(!isHide);
        }

        public bool IsCollisionWithGround()
        {
            return _countGroundCollision > 0;
        }
        private int _countGroundCollision;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == 6 )
            {
                _countGroundCollision++;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer == 6 )
            {
                _countGroundCollision--;
            }
        }

        public void SetVelocity(Vector2 velo)
        {
            rigidbody2D.velocity = velo;
        }
       
    }
}