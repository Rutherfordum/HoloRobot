namespace HoloRobot.Subscriber.JointState
{
    public interface IJointStateSubscriber : ISubscriber
    {
        public float[] GetJointPositions();
    }
}