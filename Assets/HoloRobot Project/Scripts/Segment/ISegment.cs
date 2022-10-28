namespace HoloRobot.Segment
{
    public interface ISegment
    {
        public void AddSegment(Goal.IGoal goal1, Goal.IGoal goal2);

        public void DeleteSegment(Segment segment);
    }
}