using UnityEngine;

namespace OfficeFold.Input.Interfaces
{
    public interface IInputReader
    {
        Vector2 MoveInput { get; }
        bool IsMovePressed { get; }
    }
}