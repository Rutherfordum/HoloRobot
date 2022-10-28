using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using UnityEngine.Events;

namespace HoloRobot.Goal.Cartesian
{
    public sealed class GoalCartesianRotation : MonoBehaviour, IMixedRealityPointerHandler
    {
        enum RotateAround
        {
            XAxis,
            YAxis,
            ZAxis
        }

        public UnityEvent PointerDown;
        public UnityEvent PointerUp;


        [SerializeField] GameObject rotationObject;
        [SerializeField] private bool rotateClockwise;
        [Range(1f, 3f)] [SerializeField] private float rotationalSpeed;
        [SerializeField] private RotateAround rotateAround;

        private Vector3 clickPosition;

        void PerformRotation(float value)
        {
            if (value == 0)
                return;

            float rotateFactory = value * rotationalSpeed;

            switch (rotateAround)
            {
                case RotateAround.XAxis:
                    rotationObject.transform.Rotate(rotateFactory, 0, 0);
                    break;

                case RotateAround.YAxis:
                    rotationObject.transform.Rotate(0, rotateFactory, 0);
                    break;

                case RotateAround.ZAxis:
                    rotationObject.transform.Rotate(0, 0, rotateFactory);
                    break;
            }
        }

        #region IMixedRealityPointerHandler

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            clickPosition = eventData.Pointer.Position;
            PointerDown?.Invoke();
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData)
        {
            var vectorLength = (eventData.Pointer.Position - clickPosition).magnitude;
            vectorLength = rotateClockwise ? vectorLength : vectorLength * -1;
            PerformRotation(vectorLength);
        }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
            PointerUp?.Invoke();
        }

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
        }

        #endregion
    }

}