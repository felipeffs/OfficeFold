using UnityEngine;

namespace OfficeFold.Characters.Player
{
    [CreateAssetMenu(fileName = "Player Movement Settings", menuName = "OfficeFold/Player Movement Settings",
        order = 0)]
    public class PlayerMovementSettings : ScriptableObject
    {
        [field: Header("Grounded")]
        [field: Min(0f), SerializeField]
        public float MaxWalkSpeed { get; private set; } = 5.8f;
        [field: Min(0f), SerializeField] public float WalkAcceleration { get; private set; } = 58f;
        [field: Min(0f), SerializeField] public float WalkDeceleration { get; private set; } = 58f;
        [field: Min(0f), SerializeField] public float MaxWalkableSlopeAngle { get; private set; } = 70f;

        [field: Min(0f), SerializeField] public float MaxSprintSpeed { get; private set; } = 7.8f;
        [field: Min(0f), SerializeField] public float SprintAcceleration { get; private set; } = 58f;
        [field: Min(0f), SerializeField] public float SprintDeceleration { get; private set; } = 58f;

        [field: Header("Airborne")]
        [field: Min(0f), SerializeField]
        public float MaxFallSpeed { get; private set; } = 10f;
        [field: Min(0f), SerializeField] public float MaxGroundedFallSpeed { get; private set; } = 4f;
        [field: Min(0f), SerializeField] public float FallAcceleration { get; private set; } = 9.8f;

        [field: SerializeField] public float AirborneMomentumDeceleration { get; private set; } = 29f;
    }
}