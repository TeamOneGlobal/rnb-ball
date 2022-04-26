using System;
using GamePlay.Characters;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Door
{
    [ExecuteAlways]
    public class Portal : ObjectTrigger
    {
        [SerializeField] private Transform checkMark;
        [SerializeField] private Direction direction;
        [SerializeField] private Portal other;
        [SerializeField] private Vector2 force;
        private bool _secondPortal;
        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            Debug.Log("triggerOBj : "+triggerObject.transform.position);
            Debug.Log("checkMark : "+checkMark.transform.position);
            if (_secondPortal)
            {
                _secondPortal = false;
                return;
            }
            base.TriggerEnter(triggerTag, triggerObject);
            switch (direction)
            {
                case Direction.Left:
                    if(triggerObject.transform.position.x> checkMark.position.x) return;
                    break;
                case Direction.Right:
                    if(triggerObject.transform.position.x< checkMark.position.x) return;
                    break;
                case Direction.Top:
                    if(triggerObject.transform.position.y< checkMark.position.y) return;
                    break;
                case Direction.Bottom:
                    if(triggerObject.transform.position.y> checkMark.position.y) return;
                    break;
            }
            Debug.Log("here");
            TelePort(triggerObject);
        }

        protected override void TriggerExit(string triggerTag, Transform triggerObject)
        {
            base.TriggerExit(triggerTag, triggerObject);
            _secondPortal = false;
        }
        private void TelePort(Transform character)
        {
            other.SetCharacterTeleportHere(character);
        }

        private void SetCharacterTeleportHere(Transform character)
        {
            _secondPortal = true;
            character.position = checkMark.position;
            character.GetComponent<CharacterController>().SetVelocity(Vector2.zero);
            character.GetComponent<CharacterController>().AddForce(force);
        }

        private void Init()
        {
            switch (direction)
            {
                case Direction.Left:
                    transform.localEulerAngles = new Vector3(0,0,90);
                    break;
                case Direction.Right:
                    transform.localEulerAngles = new Vector3(0,0,-90);
                    break;
                case Direction.Top:
                    transform.localEulerAngles = new Vector3(0,0,0);
                    break;
                case Direction.Bottom:
                    transform.localEulerAngles = new Vector3(0,0,180);
                    break;
            }
        }
        
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Init();
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
    }

    public enum Direction
    {
        Left,Right,Top,Bottom
    }
}