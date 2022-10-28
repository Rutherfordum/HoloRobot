using System.Collections.Generic;
using HoloRobot.Connector;
using HoloRobot.Goal;
using HoloRobot.Robot.Connector;
using UnityEngine;
using UnityEngine.Events;

namespace HoloRobot.Robot
{
    public abstract class Robot : MonoBehaviour, IRobotConnector, IRobotGoal
    {
        [Header("Robot Settings")] 
        [SerializeField] private string m_Name;
        [SerializeField] private IGoal m_GoalLINPrefab;
        [SerializeField] private HoloRobot.Segment.Segment m_SegmentLINPrefab;

        public List<IGoal> Goals = new List<IGoal>();

        protected IRosConnector RosConnector;
        
        protected readonly UnityEvent<bool> ConnectedEvent = new UnityEvent<bool>();


        public void Connect(string url, RosSharp.RosBridgeClient.Protocols.Protocol protocol, RosSharp.RosBridgeClient.RosSocket.SerializerEnum serializer)
        {
            RosConnector?.Disconnect();

            RosConnector = new RosConnector(url, protocol, serializer);
            RosConnector.ConnectedHandler += (sender, b) => ConnectedEvent?.Invoke(b);
            RosConnector?.Connect();
        }

        public void Disconnect()
        {
            RosConnector?.Disconnect();
        }

        public void AddGoal(Goal.IGoal goalPrefab)
        {
            if (goalPrefab != null)
                Goals.Add(Instantiate(goalPrefab as Transform, transform) as IGoal);
        }

        public void DeleteGoal(Goal.IGoal goalObject)
        {
            if (Goals.Contains(goalObject))
                Goals.Remove(goalObject);

            Destroy(goalObject as Transform);
        }
    }
}