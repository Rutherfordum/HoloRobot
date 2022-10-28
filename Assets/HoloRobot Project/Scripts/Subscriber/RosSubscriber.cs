using RosSharp.RosBridgeClient;
using System.Threading;
using HoloRobot.Connector;
using UnityEngine;

namespace HoloRobot.Subscriber
{
    public abstract class RosSubscriber<T> : MonoBehaviour, ISubscriber where T : Message, new()
    {
        public string Topic => _topic;
        public float TimeStep => _timeStep;

        public bool isSubscribed { get => isSubs; set => isSubs = value; }

        [SerializeField] private string _topic;
        [SerializeField] private float _timeStep = 1;

        private int secondsTimeout = 1;
        private IRosConnector _rosConnector;
        private string id;
        private bool isSubs;
        public void Subscribe(IRosConnector rosConnector)
        {
            _rosConnector = rosConnector;

            if (_rosConnector != null)
                new Thread(Subscribe).Start();
        }

        public void UnSubscribe()
        {
            if (!_rosConnector.IsConnected.WaitOne(secondsTimeout * 1000))
            {
                Debug.LogWarning("Failed to subscribe: RosConnector not connected");
                isSubs = false;
            }

            _rosConnector.Socket.Unsubscribe(id);
            isSubs = false;
        }

        private void Subscribe()
        {
            if (!_rosConnector.IsConnected.WaitOne(secondsTimeout * 1000))
            {
                isSubs = false;
                Debug.LogWarning("Failed to subscribe: RosConnector not connected");
            }

            id = _rosConnector.Socket.Subscribe<T>(_topic, ReceiveMessage, (int)(_timeStep * 1000)); // the rate(in ms in between messages) at which to throttle the topics
        }

        protected abstract void ReceiveMessage(T message);
    }
}