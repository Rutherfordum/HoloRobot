using System.Collections.Generic;
using HoloRobot.Connector;
using HoloRobot.Goal;
using HoloRobot.Robot.Connector;
using UnityEngine;
using UnityEngine.Events;

namespace HoloRobot.Robot
{
    public abstract class Robot : MonoBehaviour, IRobotConnector, IRobotCartesianGoal
    {
        #region Goal Cartesian parametrs

        [Header("Cartesian Goal Settings")]
        [Space(10)]
        [SerializeField] private string m_Name;
        [SerializeField] private Goal.Goal m_CartesianGoal;
        private List<Goal.Goal> m_CartesianGoals;

        public string Name => m_Name;
        public Goal.Goal CartesianGoalPrefab => m_CartesianGoal;
        public List<Goal.Goal> CartesianGoals => m_CartesianGoals;

        #endregion

        #region Connector parametrs

        protected IRosConnector RosConnector;
        protected readonly UnityEvent<bool> ConnectedEvent = new UnityEvent<bool>();

        #endregion

        #region Goal Cartesian Methods

        public void AddCartesianGoal()
        {
            if (m_CartesianGoal != null)
            {
                Goal.Goal goal = Instantiate(m_CartesianGoal.gameObject, transform).GetComponent<Goal.Goal>();
                m_CartesianGoals.Add(goal);
            }
        }
        public void DeleteCartesianGoal(Goal.Goal goalObject)
        {
            if (m_CartesianGoals.Contains(goalObject))
                m_CartesianGoals.Remove(goalObject);

            Destroy(goalObject.gameObject);
        }
        public void ClearCartesianGoal()
        {
            foreach (var goal in m_CartesianGoals)
                Destroy(goal);

            m_CartesianGoals?.Clear();
        }

        #endregion

        #region Connector Methods

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

        #endregion
    }
}