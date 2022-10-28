using HoloRobot.Goal.Gripper;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UnityEngine;

namespace HoloRobot.Goal.Cartesian
{
    [RequireComponent(typeof(TapToPlace), typeof(ObjectManipulator))]
    public sealed class GoalCartesian : MonoBehaviour, IGoal, IGripper
    {
        public bool isLocked => locked;
        
        public bool isGripper
        {
            set
            {
                gripperActive = value;

                if (value == false)
                {
                    gripperPosCount = -1;
                    ChangeGripperState();
                }
            }
            get => gripperActive;
        }

        /// <summary>
        /// Current gripper state
        /// </summary>
        public GripperState GetGripperState => gripperState;
        private bool locked;
        private bool gripperActive;
        private GripperState gripperState = GripperState.Nothing;
        private int gripperPosCount;
        private Collider[] colliders;
        private TapToPlace tapToPlace;
        private Color defaultColor = Color.magenta;

        [SerializeField] private PointerHandler hideButton;
        [SerializeField] private GameObject hideObject;

        [SerializeField] private PointerHandler gripperButton;
        [SerializeField] private GameObject gripperOpenObject;
        [SerializeField] private GameObject gripperCloseObject;

        private Material[] changeColorMaterials;

#if UNITY_EDITOR
        private void OnValidate()
        {
            tapToPlace = this.gameObject?.GetComponent<TapToPlace>();
            colliders = this.gameObject?.GetComponentsInChildren<Collider>();
            changeColorMaterials = GetAllChangeMaterials();
        }
#endif

        private void Start()
        {
            hideButton?.OnPointerClicked?.AddListener(ChangeHideState);
            gripperButton?.OnPointerClicked?.AddListener(ChangeGripperState);
            tapToPlace?.OnPlacingStarted?.AddListener(PlacingStart);
            tapToPlace?.OnPlacingStopped?.AddListener(PlacingStop);
        }

        
        public void Hide(bool value)
        {
            if (locked) return;
            value = !value;

            colliders[0].enabled = value;
            hideObject.SetActive(value);

            var z = value ? -0.2358f : -0.0647f;
            hideButton.transform.localPosition = new Vector3(0, 0, z);
        }

        /// <summary>
        /// UnLocked move goal
        /// </summary>
        public void UnLock()
        {
            Hide(false);
            SwitchingColliders(true);
            ChangeColor(defaultColor);
            locked = false;
        }

        /// <summary>
        /// Locked move goal
        /// </summary>
        public void Lock()
        {
            Hide(true);
            SwitchingColliders(false);
            ChangeColor(Color.blue);
            locked = true;
        }

        #region Private Methods

        private void PlacingStart()
        {
            SwitchingColliders(false);
        }

        private void PlacingStop()
        {
            SwitchingColliders(true);
            tapToPlace.enabled = false;
        }

        private void ChangeHideState(MixedRealityPointerEventData arg0)
        {
            Hide(hideObject.activeSelf);
        }

        private void ChangeGripperState(MixedRealityPointerEventData arg0)
        {
            if (!isGripper)
                return;

            ChangeGripperState();
        }

        private void SwitchingColliders(bool value)
        {
            foreach (var col in colliders)
                col.enabled = value;
        }

        private void ChangeGripperState()
        {
            gripperPosCount++;

            switch (gripperPosCount)
            {
                case 0:
                    gripperState = GripperState.Nothing;
                    gripperOpenObject.SetActive(false);
                    gripperCloseObject.SetActive(false);
                    break;

                case 1:
                    gripperState = GripperState.Open;
                    gripperOpenObject.SetActive(true);
                    gripperCloseObject.SetActive(false);
                    break;

                case 2:
                    gripperState = GripperState.Close;
                    gripperOpenObject.SetActive(false);
                    gripperCloseObject.SetActive(true);
                    gripperPosCount = -1;
                    break;

                default:
                    gripperState = GripperState.Nothing;
                    gripperOpenObject.SetActive(false);
                    gripperCloseObject.SetActive(false);
                    break;
            }
        }

        private void ChangeColor(Color color)
        {
            foreach (var mat in changeColorMaterials)
                mat.color = color;
        }

        private Material[] GetAllChangeMaterials()
        {
            Material[] materials;
            Renderer[] renders;

            renders = gripperButton.GetComponentsInChildren<Renderer>(true);
            materials = new Material[renders.Length + 1];

            for (var index = 0; index < renders.Length; index++)
                materials[index + 1] = renders[index].sharedMaterial;

            materials[0] = hideButton.GetComponent<Renderer>().sharedMaterial;

            return materials;
        }

        #endregion
    }
}