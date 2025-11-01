using OfficeFold.Input.Interfaces;
using UnityEngine;

namespace EditModeTests.Mock
{
    public class InputReaderMock : IInputReader
    {
        public Vector3 MoveInput { get; set; }
        public bool IsMovePressed { get; set; }
    }
}