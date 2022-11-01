using UnityEngine;

namespace Robot.IK
{
    public class JointIK : MonoBehaviour
    {
        private enum AxisRotation
        {
            Nothing,
            X,
            Y,
            Z
        }

        [SerializeField] private AxisRotation m_AxisRotation;

#if UNITY_EDITOR
        void OnValidate()
        {
            switch (m_AxisRotation)
            {
                case AxisRotation.X:
                    axis = Vector3.right;
                    break;

                case AxisRotation.Y:
                    axis = Vector3.up;
                    break;

                case AxisRotation.Z:
                    axis = Vector3.forward;
                    break;

                case AxisRotation.Nothing:
                    axis = Vector3.zero;
                    break;
            }
        }
#endif

        [HideInInspector] public Vector3 Axis => axis;

        [HideInInspector] public Vector3 StartOffsetPos => startOffsetPos;

        [HideInInspector] public Quaternion StartOffsetOri => startOffsetOri;

        private Vector3 startOffsetPos;
        private Quaternion startOffsetOri;
        private Vector3 axis;


        public void Awake()
        {
            startOffsetPos = transform.localPosition;
            startOffsetOri = transform.localRotation;
        }
    }
}
