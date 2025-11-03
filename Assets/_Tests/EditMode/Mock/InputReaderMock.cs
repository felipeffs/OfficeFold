using OfficeFold.Input.Interfaces;
using UnityEngine;

namespace EditModeTests.Mock
{
    public class InputReaderMock : IInputReader
    {
        public Vector2 MoveInput { get; set; }
        public bool IsMovePressed { get; set; }
    }
}