using OfficeFold.Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OfficeFold.Input
{
    public class InputReader : IInputReader
    {
        private readonly InputAction _moveAction;
        private const string MoveActionId = "351f2ccd-1f9f-44bf-9bec-d62ac5c5f408";

        public InputReader(InputActionAsset inputActionsAsset)
        {
            _moveAction = inputActionsAsset.FindAction(MoveActionId);
            _moveAction.Enable();
        }

        public Vector2 MoveInput => _moveAction.ReadValue<Vector2>();
        public bool IsMovePressed => _moveAction.IsPressed();
    }
}