namespace HoloRobot.Segment
{
    public interface ISegment
    {
        public void AddSegment(Goal.Goal goal1, Goal.Goal goal2);

        public void DeleteSegment(Segment segment);
    }
}