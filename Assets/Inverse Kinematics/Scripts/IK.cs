using System.Collections.Generic;
using UnityEngine;

namespace Robot.IK
{
    public class IK : MonoBehaviour
    {
        [Tooltip("Object with tree structure for IK solution")]
        [SerializeField]
        private GameObject armature;

        public Transform Target;

        [Tooltip("Max speed")]
        [Range(0.01f, 0.1f)]
        [SerializeField]
        private float maxSpeed = 0.1f;

        [SerializeField]
        private float distanceThreshold = 0.001f; 
        [SerializeField]
        private float samplingDistance = 0.1f; 

        private float speedRate = 100; 
        [SerializeField]
        private float error = 0.1f;
        private bool playIK = true;

        [SerializeField] private List<JointIK> joints = new List<JointIK>();
        float[] angles; 


#if UNITY_EDITOR
        private void OnValidate()
        {
            joints.Clear();
            angles = new float[0];

            if (!armature)
            {
                return;
            }

            var jointsIk = armature.GetComponentsInChildren<JointIK>();

            if (jointsIk.Length != 0)
                joints.AddRange(jointsIk);

            angles = new float[joints.Count];
        }
#endif

        public void Pause()
        {
            playIK = false;
        }

        public void Play()
        {
            playIK = true;
        }

        
        private void Update()
        {
          
            if (!playIK) return;
            
            InverseKinematics(Target.position, Target.rotation);
            
            ApplyAngles();

        }

        private void ApplyAngles()
        {
            for (int i = 0; i < joints.Count; i++)
                joints[i].transform.localRotation =
                    joints[i].StartOffsetOri * Quaternion.AngleAxis(angles[i], joints[i].Axis);
        }

        private void InverseKinematics(Vector3 target, Quaternion targetEuler)
        {
            error = ErrorFunction(target, targetEuler);

            if (error < distanceThreshold)
                return;

            for (int i = joints.Count - 1; i >= 0; i--)
            {
                float gradient = PartialGradient(target, targetEuler, i);

                speedRate = 10 * Mathf.Pow(1000, error);
                angles[i] -= Mathf.Clamp(speedRate * gradient, -maxSpeed, maxSpeed);

                if (ErrorFunction(target, targetEuler) < distanceThreshold)
                    return;
            }
        }

        private float PartialGradient(Vector3 target, Quaternion targetEuler, int i)
        {
            float angle = angles[i];

            float f_x = ErrorFunction(target, targetEuler);
            angles[i] += samplingDistance;
            float f_xPlusSampligDistance = ErrorFunction(target, targetEuler);

            float gradient = (f_xPlusSampligDistance - f_x) / samplingDistance;

            angles[i] = angle;

            return gradient;
        }

        private float ErrorFunction(Vector3 target, Quaternion targetEuler)
        {
            var point = ForwardKinematics();
            float distancePenalty = Vector3.Distance(target, point.Item1);
            float rotationPenalty = Quaternion.Angle(targetEuler, point.Item2) / 180;
            return distancePenalty + rotationPenalty;
        }

        private (Vector3, Quaternion) ForwardKinematics()
        {
            Quaternion rotation = joints[0].transform.rotation 
                                  * Quaternion.Inverse(joints[0].StartOffsetOri);

            Vector3 prevPoint = joints[0].transform.position;

            for (int i = 1; i < joints.Count; i++)
            {
                rotation *= joints[i - 1].StartOffsetOri
                            * Quaternion.AngleAxis(angles[i - 1], joints[i - 1].Axis);

                Vector3 nextPoint = prevPoint + rotation * joints[i].StartOffsetPos;

                prevPoint = nextPoint;
            }

            rotation *= joints[^1].StartOffsetOri;

            return (prevPoint, rotation);
        }
    }
}
