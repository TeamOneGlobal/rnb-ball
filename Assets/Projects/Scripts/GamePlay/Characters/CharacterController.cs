using System;
using Projects.Scripts;
using UnityEngine;

namespace GamePlay.Characters
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private new CharacterAnimation animation;
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterMagnetic magnetic;
        [SerializeField] private CharacterSelected selected;
        [SerializeField] private CharacterHealthPoint healthPoint;

        
        private void Awake()
        {
            animation.Init(this);
            movement.Init(this);
            magnetic.Init(this);
            selected.Init(this);
            healthPoint.Init(this);
        }

        private void Start()
        {
            PlayIdle();
        }

        #region Movement

        public void Jump()
        {
            movement.Jump().Forget();
        }
        public void MoveLeft(bool isMove)
        {
            if(isMove)
                movement.MoveLeft();
            else 
                movement.EndMoveLeft();
        }
        public void MoveRight(bool isMove)
        {
            if(isMove)
                movement.MoveRight();
            else 
                movement.EndMoveRight();
        }
        public void AddForce(Vector2 force)
        {
            if(healthPoint.IsDie()) return;
            movement.AddForce(force);
        }
        public Vector2 GetVelocity()
        {
            return movement.GetVelocity();
        }

        public void SetVelocity(Vector2 velo)
        {
            movement.SetVelocity(velo);
        }
        public void CancelAllMove()
        {
            MoveLeft(false);
            MoveRight(false);
        }

        public bool IsJumping()
        {
            return movement.IsJumping();
        }
        public void SetOnWater(bool value)
        {
            movement.SetOnWater(value);
        }

        public bool CollisionWithGround()
        {
            return movement.IsCollisionWithGround();
        }
        #endregion

        #region Magnet

        public void ActiveMagnetic(bool isActive)
        {
            magnetic.ActiveMagnetic(isActive);
        }

        #endregion

        #region Selected

        public void OnCharacterSelected(bool select)
        {
            if(select)
                selected.OnCharacterSelected();
            else
                selected.OnCharacterUnSelected();
        }

        #endregion

        #region Animation

        public void PlayIdle()
        {
            animation.PlayIdle();
        }

        public void PlayDie(Action complete =null)
        {
            animation.PlayDie(complete);
        }

        public void PlayCallAnim()
        {
            animation.PlayCallAnim();
        }

        public void PlayWinAnim()
        {
            animation.PlayWinAnim();
        }
        #endregion

        #region Health Point



        public void Revive(Action onComplete = null)
        {
            healthPoint.Revive(onComplete);
        }

        public void Damage(CheckPoint checkPoint,Vector2 force, int damage = 1)
        {
            AddForce(force);
            healthPoint.Damage(checkPoint,damage);
        }
        #endregion

        public void HideBall(bool isHide)
        {
            movement.HideBall(isHide);
            if (isHide)
                magnetic.ActiveMagnetic(false);
            else
            {
                if(GameDataManager.Instance.GetMagnetDuration()>0)
                    magnetic.ActiveMagnetic(true);
            }
            selected.HideBall(isHide);
        }



        public void TrySkin(string skinName)
        {
            animation.TrySkin(skinName);
        }
    }
}