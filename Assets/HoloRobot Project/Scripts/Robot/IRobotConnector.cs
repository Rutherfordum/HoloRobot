namespace HoloRobot.Robot.Connector
{
    public interface IRobotConnector
    {
        public void Connect(string url, RosSharp.RosBridgeClient.Protocols.Protocol protocol, RosSharp.RosBridgeClient.RosSocket.SerializerEnum serializer);

        public void Disconnect();
    }
}