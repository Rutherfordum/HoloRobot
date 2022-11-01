using UnityEngine;

namespace HoloRobot.Goal
{
    public abstract class Goal: MonoBehaviour
    {
        public abstract void Hide(bool value);

        public abstract void Lock();

        public abstract void UnLock();
    }
}