using HoloRobot.Connector;

namespace HoloRobot.Subscriber
{
    public interface ISubscriber
    {
        public bool isSubscribed { get; set; }
        public void Subscribe(IRosConnector rosConnector);
        public void UnSubscribe();
    }
}