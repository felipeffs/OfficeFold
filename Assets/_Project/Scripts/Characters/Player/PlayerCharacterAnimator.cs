using UnityEngine;

namespace OfficeFold.Characters.Player
{
    public class PlayerCharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerCharacter _playerCharacter;
        [SerializeField] private Transform _meshGroupTransform;
        [SerializeField] private float _rotationSpeed;

        [SerializeField] private float _animationTransitionSmoothDelta = 5.5f;

        private readonly int _pSpeed = Animator.StringToHash("SpeedRatio");
        private float _horizontalSpeedRatio;

        private float _currentSpeedChangeAmount;

        private Vector3 _facingDirection;

        public void Update()
        {
            _horizontalSpeedRatio = Mathf.SmoothDamp(_horizontalSpeedRatio, _playerCharacter.GetHorizontalSpeedRatio(),
                ref _currentSpeedChangeAmount, _animationTransitionSmoothDelta * Time.deltaTime);

            if (_playerCharacter.MoveDirection != Vector3.zero)
                _facingDirection = _playerCharacter.MoveDirection;
            
            RotateVisualTowardsDirection(_facingDirection);

            _animator.SetFloat(_pSpeed, _horizontalSpeedRatio);
        }

        private void RotateVisualTowardsDirection(Vector3 direction)
        {
            var lookRotation = Quaternion.LookRotation(direction);
            _meshGroupTransform.rotation = Quaternion.Slerp(_meshGroupTransform.rotation, lookRotation,
                _rotationSpeed * Time.deltaTime);
        }
    }
}