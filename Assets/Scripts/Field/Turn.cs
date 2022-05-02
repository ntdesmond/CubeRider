using System;
using UnityEngine;

namespace Field
{
    public class Turn : MonoBehaviour
    {
        [Serializable]
        public enum TurnDirection
        {
            Left, Right
        }

        public TurnDirection direction;
    }
}