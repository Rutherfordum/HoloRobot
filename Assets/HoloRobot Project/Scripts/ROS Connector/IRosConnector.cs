using System;
using System.Threading;
using RosSharp.RosBridgeClient;

namespace HoloRobot.Connector
{
    public interface IRosConnector
    {
        public RosSocket Socket { get; set; }
        public ManualResetEvent IsConnected { get; set; }
        public EventHandler<bool> ConnectedHandler { get; set; }
        public bool isConnected { get; set; } 

        public void Connect();
        public void Disconnect();
    }
}