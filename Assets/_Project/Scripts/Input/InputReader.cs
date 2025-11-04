using OfficeFold.Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OfficeFold.Input
{
    public class InputReader : IInputReader
    {
        private readonly InputAction _moveAction;
        private readonly InputAction _sprintAction;
        private const string MoveActionId = "351f2ccd-1f9f-44bf-9bec-d62ac5c5f408";
        private const string SprintActionId = "641cd816-40e6-41b4-8c3d-04687c349290";

        public InputReader(InputActionAsset inputActionsAsset)
        {
            _moveAction = inputActionsAsset.FindAction(MoveActionId);
            _sprintAction = inputActionsAsset.FindAction(SprintActionId);

            _moveAction.Enable();
            _sprintAction.Enable();
        }


        public Vector2 MoveInput => _moveAction.ReadValue<Vector2>();
        public bool IsMovePressed => _moveAction.IsPressed();
        public bool IsSprintInputPressed => _sprintAction.IsPressed();
    }
}