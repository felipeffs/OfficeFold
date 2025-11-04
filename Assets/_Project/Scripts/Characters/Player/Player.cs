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
            HandleMovement(inputs.MoveVector, inputs.IsSprintPressed);
        }

        private void HandleMovement(Vector3 direction, bool isSprintPressed)
        {
            if (_isGrounded)
            {
                var hasSprintInput = isSprintPressed && direction.magnitude > 0;
                var isAtSprintSpeed = _moveVelocity.magnitude - _movementSettings.MaxWalkSpeed > .1f;

                if (hasSprintInput)
                {
                    Debug.Log($"<color=#00FF00>Grounded/️Sprint |{_moveVelocity.magnitude}</color>");
                    _moveVelocity = CalculateSprintVelocity(direction);
                }
                else if (isAtSprintSpeed)
                {
                    Debug.Log($"<color=#F25333>Grounded/SprintToWalk |{_moveVelocity.magnitude}</color>");
                    _moveVelocity = CalculateSprintToWalkDecelerationVelocity();
                }
                else
                {
                    Debug.Log($"<color=#666EFF>Grounded/Walk |{_moveVelocity.magnitude}</color>");
                    _moveVelocity = CalculateWalkVelocity(direction);
                }

                _fallVelocity = CalculateGroundedFallVelocity();
            }
            else
            {
                Debug.Log("<color=#F2E533>Airborne/Fall</color>");
                _moveVelocity = CalculateAirborneMomentumVelocity();
                _fallVelocity = CalculateFallVelocity();
            }

            _rb.linearVelocity = _moveVelocity + _fallVelocity;
        }

        private Vector3 CalculateWalkVelocity(Vector3 direction)
        {
            var targetVelocity = direction * _movementSettings.MaxWalkSpeed;

            var velocityDot = Vector3.Dot(_moveVelocity.normalized, targetVelocity.normalized);
            var acceleration = (velocityDot > 0f
                ? _movementSettings.WalkAcceleration
                : _movementSettings.WalkDeceleration) * Time.fixedDeltaTime;

            return Vector3.MoveTowards(_moveVelocity, targetVelocity, acceleration);
        }

        private Vector3 CalculateSprintVelocity(Vector3 direction)
        {
            var targetVelocity = direction.normalized * _movementSettings.MaxSprintSpeed;

            var velocityDot = Vector3.Dot(_moveVelocity.normalized, targetVelocity.normalized);
            var acceleration = (velocityDot > 0f
                ? _movementSettings.SprintAcceleration
                : _movementSettings.SprintDeceleration) * Time.fixedDeltaTime;

            return Vector3.MoveTowards(_moveVelocity, targetVelocity, acceleration);
        }

        private Vector3 CalculateSprintToWalkDecelerationVelocity()
        {
            var acceleration = _movementSettings.SprintDeceleration * Time.fixedDeltaTime;
            return Vector3.MoveTowards(_moveVelocity, Vector3.zero, acceleration);
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