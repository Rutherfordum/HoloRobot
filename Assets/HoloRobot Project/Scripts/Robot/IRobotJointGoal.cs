namespace HoloRobot.Robot.Industrial
{
    public interface IRobotJointGoal
    {
        public void AddJointGoal();

        public void DeleteJointGoal(Goal.Goal goalObject);

        public void ClearJointGoal();
    }
}