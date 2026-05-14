using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Unity.FantasyKingdom
{
    public class MoveTarget : MonoBehaviour
    {
        public InputActionAsset ActionAsset;
        private InputAction m_PlayerMove;
        private InputAction m_LeftMousePress;
        private InputAction m_scrollHeight;
        private Vector2 m_position;
        private Vector2 m_height;

        private float m_minHeight = 10;
        public float ZoomSpeed = 3;

        public CinemachineCamera MovingCamera;
        public CinemachineCamera RotateCamera;

        private void OnEnable()
        {
            ActionAsset.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            ActionAsset.FindActionMap("Player").Disable();
        }

        private void Start()
        {
            m_PlayerMove = ActionAsset.FindAction("Move");
            m_LeftMousePress = ActionAsset.FindAction("Attack");
            m_scrollHeight = ActionAsset.FindAction("Scroll");
        }

        private void Update()
        {
            
            m_position = m_PlayerMove.ReadValue<Vector2>();
            if (transform.position.y > m_minHeight)
            {
                m_height = m_scrollHeight.ReadValue<Vector2>();
            }
            else
            {
                m_height.y = m_minHeight;
            }

            
            transform.Translate(m_position.x, m_height.y * ZoomSpeed, m_position.y);

            if (m_LeftMousePress.WasPressedThisFrame())
            {
                RotateCamera.Priority = 2;
            }
            if (m_LeftMousePress.WasReleasedThisFrame())
            {
                RotateCamera.Priority = 0;
            }
        }
    }
}

