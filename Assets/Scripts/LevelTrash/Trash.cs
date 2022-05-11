using GameFlow;
using UnityEngine;

namespace LevelTrash
{
    public class Trash : MonoBehaviour
    {
        public void Construct(GameEvents gameEvents)
        {
            gameEvents.LevelStarted += EmptySelf;
        }

        private void EmptySelf()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}