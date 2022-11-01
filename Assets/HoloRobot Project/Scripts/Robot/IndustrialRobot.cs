using System.Collections.Generic;
using HoloRobot.Subscriber;
using HoloRobot.Subscriber.JointState;
using UnityEngine;

namespace HoloRobot.Robot.Industrial
{
    public class IndustrialRobot : Robot, IRobotJointGoal
    {
        #region Joint Goal parametrs

        public Goal.Goal JointGoalPrefab => m_JointGoalPrefab;
        public List<Goal.Goal> JointGoals => m_JointGoals;

        [SerializeField] private Goal.Goal m_JointGoalPrefab;
        private List<Goal.Goal> m_JointGoals;
        
        #endregion

        #region Joint State parametrs

        [Header("Joint State Settings")]
        [Space(10)]
        [SerializeField] protected List<Transform> Joints;
        [SerializeField] protected Vector3[] JointsPivot;
        [SerializeField] [Range(10f, 50f)] protected float Speed = 10;
        [SerializeField] private MonoBehaviour jointStateSubscriber;

        protected Quaternion[] JointsOffset;

        protected IJointStateSubscriber JointStateSubscriber => jointStateSubscriber as IJointStateSubscriber;

        #endregion

        #region Unity Editor

#if UNITY_EDITOR
        void OnValidate()
        {
            JointsOffset = new Quaternion[Joints.Count];
            
            for (var i = 0; i < Joints.Count; i++)
                JointsOffset[i] = Joints[i].localRotation;

            if (!(jointStateSubscriber is ISubscriber) && (jointStateSubscriber is IJointStateSubscriber))
            {
                Debug.LogError(jointStateSubscriber.name + " is not subscriber");
                jointStateSubscriber = null;
            }

        }
#endif
        #endregion

        #region Joint Goal Methods

        public void AddJointGoal()
        {
            if (m_JointGoalPrefab != null)
            {
                Goal.Goal goal = Instantiate(m_JointGoalPrefab.gameObject, transform).GetComponent<Goal.Goal>();
                m_JointGoals.Add(goal);
            }
        }

        public void DeleteJointGoal(Goal.Goal goalObject)
        {
            if (m_JointGoals.Contains(goalObject))
                m_JointGoals.Remove(goalObject);

            Destroy(goalObject.gameObject);
        }

        public void ClearJointGoal()
        {
            foreach (var goal in m_JointGoals)
                Destroy(goal);

            m_JointGoals?.Clear();
        }
        
        #endregion
    }
}