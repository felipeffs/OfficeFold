using UnityEngine;

namespace OfficeFold.Input.Interfaces
{
    public interface IInputReader
    {
        Vector3 MoveInput { get; }
        bool IsMovePressed { get; }
    }
}