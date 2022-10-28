using System.Collections.Generic;
using HoloRobot.Subscriber;
using HoloRobot.Subscriber.JointState;
using UnityEngine;

namespace HoloRobot.Robot.Industrial
{
    public class IndustrialRobot : Robot
    {
        [Header("Robot Settings")]
        [SerializeField] private Goal.IGoal m_JointGoalPrefab;
        [SerializeField] private HoloRobot.Segment.Segment m_SegmentPTPPrefab;

        [Header("Joint State Settings")]
        [Space(10)]
        [SerializeField] protected List<Transform> Joints;
        [SerializeField] protected Vector3[] JointsPivot;
        [SerializeField] [Range(10f, 50f)] protected float Speed = 10;
        [SerializeField] private MonoBehaviour jointStateSubscriber;



        protected Quaternion[] JointsOffset;

        protected IJointStateSubscriber JointStateSubscriber => jointStateSubscriber as IJointStateSubscriber;

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
    }
}