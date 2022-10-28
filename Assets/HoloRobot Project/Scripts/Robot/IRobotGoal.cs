using HoloRobot.Robot.Program;

namespace HoloRobot.Goal
{
    public interface IRobotGoal
    {
        public void AddGoal(IGoal goalPrefab);

        public void DeleteGoal(IGoal goalObject);
    }
}