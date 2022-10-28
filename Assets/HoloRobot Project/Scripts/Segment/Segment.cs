using UnityEngine;

namespace HoloRobot.Segment
{
    public abstract class Segment: MonoBehaviour
    {
        protected Segment(Goal.IGoal goal1, Goal.IGoal goal2)
        {
            _goal1 = goal1 as Transform;
            _goal2 = goal2 as Transform;
        }

        private Transform _goal1;
        private Transform _goal2;

        private void Update()
        {
            SetSegmentBetweenGoals(_goal1, _goal2);
        }

        private void SetSegmentBetweenGoals(Transform goal1, Transform goal2)
        {
            if (goal1 == null || goal2 == null)
                Destroy(gameObject);
            
            else
            {
                Vector3 direction = goal2.transform.position - goal1.transform.position;
                Vector3 centerPos = (goal2.transform.position + goal1.transform.position) * 0.5f;

                transform.position = centerPos;
                transform.rotation = Quaternion.LookRotation(direction);

                float defaultLineScale = 0.005f;
                float distance = Vector3.Distance(goal1.transform.position, goal2.transform.position);

                transform.localScale = new Vector3(distance, defaultLineScale, defaultLineScale);
                transform.Rotate(Vector3.down, 90f);
            }
        }
    }
}