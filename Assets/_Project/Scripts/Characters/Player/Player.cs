using OfficeFold.Input;
using OfficeFold.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OfficeFold.Characters.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Player : MonoBehaviour
    {
        private PlayerInputHandler _inputHandler;

        [Header("Components")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private CapsuleCollider _col;

        [Header("Design Settings")]
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private PlayerMovementSettings _movementSettings;

        [Header("Ground Detection")]
        [SerializeField] private float _groundCheckDistance = .1f;
        [SerializeField] private float _skinWidth = .01f;

        private Vector3 _moveVelocity;
        private Vector3 _fallVelocity;
        private bool _isGrounded;

        private void OnValidate()
        {
            _rb ??= GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            var inputReader = new InputReader(_inputActions);
            _inputHandler = new PlayerInputHandler(inputReader, Camera.main);
        }

        private void Start()
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _rb.isKinematic = false;
            _rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            _isGrounded = IsGrounded();
            var inputs = _inputHandler.GetInputs();
            HandleMovement(inputs.MoveVector);
        }

        private void HandleMovement(Vector3 direction)
        {
            if (_isGrounded)
            {
                _moveVelocity = CalculateGroundedMovementVelocity(direction);
                _fallVelocity = CalculateGroundedFallVelocity();
            }
            else
            {
                _moveVelocity = CalculateAirborneMomentumVelocity();
                _fallVelocity = CalculateFallVelocity();
            }

            _rb.linearVelocity = _moveVelocity + _fallVelocity;
        }

        private Vector3 CalculateGroundedMovementVelocity(Vector3 direction)
        {
            var targetVelocity = direction * _movementSettings.MaxWalkSpeed;

            var velocityDot = Vector3.Dot(_moveVelocity.normalized, targetVelocity.normalized);
            var acceleration = (velocityDot > 0f
                ? _movementSettings.WalkAcceleration
                : _movementSettings.WalkDeceleration) * Time.fixedDeltaTime;

            return Vector3.MoveTowards(_moveVelocity, targetVelocity, acceleration);
        }

        private Vector3 CalculateAirborneMomentumVelocity()
        {
            var targetVelocity = Vector3.zero;
            var acceleration = _movementSettings.AirborneMomentumDeceleration * Time.fixedDeltaTime;

            return Vector3.MoveTowards(_moveVelocity, targetVelocity, acceleration);
        }

        private Vector3 CalculateGroundedFallVelocity()
        {
            var targetVelocity = Vector3.down * _movementSettings.MaxGroundedFallSpeed;
            var gravityAcceleration = _movementSettings.FallAcceleration * Time.deltaTime;

            return Vector3.MoveTowards(_fallVelocity, targetVelocity, gravityAcceleration);
        }

        private Vector3 CalculateFallVelocity()
        {
            var targetVelocity = Vector3.down * _movementSettings.MaxFallSpeed;
            var gravityAcceleration = _movementSettings.FallAcceleration * Time.deltaTime;

            return Vector3.MoveTowards(_fallVelocity, targetVelocity, gravityAcceleration);
        }

        private bool IsGrounded()
        {
            var radius = _col.radius - _skinWidth;
            var origin = transform.position.Add(y: radius);
            var gravityDirection = Vector3.down;

            var hits = new RaycastHit[2];
            var size = Physics.SphereCastNonAlloc(origin, radius, gravityDirection, hits, _groundCheckDistance);

            for (var i = 0; i < size; i++)
            {
                if (_col == hits[i].collider)
                    continue;

                var angle = Vector3.Angle(hits[i].normal, gravityDirection * -1);

                if (angle <= _movementSettings.MaxWalkableSlopeAngle)
                {
                    return true;
                }
            }

            return false;
        }
    }
}