namespace HoloRobot.Goal
{
    public interface IRobotCartesianGoal
    {
        public void AddCartesianGoal();

        public void DeleteCartesianGoal(Goal goalObject);

        public void ClearCartesianGoal();
    }
}