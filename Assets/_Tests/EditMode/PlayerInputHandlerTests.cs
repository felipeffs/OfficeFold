using EditModeTests.Mock;
using NUnit.Framework;
using OfficeFold.Input;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace EditModeTests
{
    public class PlayerInputHandlerTests
    {
        private Camera _camera;
        private Vector3EqualityComparer _vector3EqualityComparer;

        [SetUp]
        public void Startup()
        {
            _vector3EqualityComparer = new Vector3EqualityComparer(.009f);
            _camera = new GameObject().AddComponent<Camera>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_camera.gameObject);
        }

        [Test]
        public void GetInputsMoveVector_CameraRotatedOnYAndZAxes_ReturnsMoveDirectionRelativeToCamera()
        {
            // Arrange
            var cameraRotationOnYAxis = Quaternion.AngleAxis(-45f, Vector3.up);
            var cameraRotationOnZAxis = Quaternion.AngleAxis(30f, Vector3.forward);
            _camera.transform.rotation = cameraRotationOnYAxis * cameraRotationOnZAxis;

            var inputReader = new InputReaderMock
            {
                MoveInput = Vector3.right,
                IsMovePressed = true
            };

            var playerInputHandler = new PlayerInputHandler(inputReader, _camera);

            // Act
            var inputs = playerInputHandler.GetInputs();

            // Assert
            var expectedDirection = new Vector3(.71f, 0, .71f);
            Assert.That(inputs.MoveVector, Is.EqualTo(expectedDirection).Using(_vector3EqualityComparer));
        }

        [Test]
        public void GetInputsMoveVector_MoveIsNotPressedAndRawMoveInputIsNotZeroed_ReturnsZeroedMoveDirection()
        {
            // Arrange
            var inputReader = new InputReaderMock
            {
                MoveInput = new Vector3(1, .5f, .1f),
                IsMovePressed = false
            };

            var playerInputHandler = new PlayerInputHandler(inputReader, _camera);

            // Act
            var inputs = playerInputHandler.GetInputs();

            // Assert
            Assert.That(inputs.MoveVector, Is.EqualTo(Vector3.zero).Using(_vector3EqualityComparer));
        }
    }
}