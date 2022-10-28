using UnityEngine;

namespace HoloRobot.Goal
{
    public interface IGoal
    {
        public void Hide(bool value);

        public void Lock();

        public void UnLock();
    }
}