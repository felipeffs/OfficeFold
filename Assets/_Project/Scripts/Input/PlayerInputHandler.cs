using OfficeFold.Input.Interfaces;
using UnityEngine;

namespace OfficeFold.Input
{
    public class PlayerInputHandler
    {
        public PlayerInputHandler(IInputReader inputReader, Camera camera)
        {
            _inputReader = inputReader;
            _camera = camera;
        }

        private readonly IInputReader _inputReader;
        private readonly Camera _camera;

        public PlayerInputs GetInputs()
        {
            var inputs = new PlayerInputs
            {
                MoveVector = GetMoveVectorRelativeToCamera()
            };

            return inputs;
        }

        private Vector3 GetMoveVectorRelativeToCamera()
        {
            if (!_inputReader.IsMovePressed) return Vector3.zero;

            var rawMoveInput = _inputReader.MoveInput;

            var cameraForward = _camera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            var cameraRight = _camera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            var forward = cameraForward * rawMoveInput.y;
            var right = cameraRight * rawMoveInput.x;

            return forward + right;
        }
    }
}