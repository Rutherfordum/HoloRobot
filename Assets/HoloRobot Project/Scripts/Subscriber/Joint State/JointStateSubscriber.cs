using HoloRobot.Utils;
using JointState_msgs = RosSharp.RosBridgeClient.MessageTypes.Sensor.JointState;

namespace HoloRobot.Subscriber.JointState
{
    public sealed class JointStateSubscriber: RosSubscriber<JointState_msgs>, IJointStateSubscriber
    {
        public float[] GetJointPositions() => positions;

        private float[] positions;

        protected override void ReceiveMessage(JointState_msgs message)
        {
            positions = Convertor.DoubleToFloatArray(message.position);
            var pos = positions[0];
            positions[0] = positions[2];
            positions[2] = pos;
            isSubscribed = true;
        }
    }
}