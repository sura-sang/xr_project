using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuraSang
{
    public class Happiness : Monster
    {
        public override Emotion Emotion => Emotion.Happiness;
        
        [SerializeField] private Transform _moveRange;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();

            Agent.updateRotation = false;

            FineNearestTag();

            ChangeState(new HappinessIdle(this));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (_moveRange != null)
                Gizmos.DrawLine(transform.position, _moveRange.transform.position);
        }

        private void FineNearestTag()
        {
            GameObject[] moveRange = GameObject.FindGameObjectsWithTag("MoveRange");
            float shortDis = 0;

            if (moveRange.Length != 0)
            {
                shortDis = Vector3.Distance(transform.position, moveRange[0].transform.position);
            }

            foreach (GameObject mRange in moveRange)
            {
                var dist = Vector3.Distance(transform.position, mRange.transform.position);

                if (dist <= shortDis)
                {
                    _moveRange = mRange.transform;
                    shortDis = dist;
                }
            }
        }

        public void SleepSound()
        {
            AudioManager.Instance.SoundOneShot3D(AudioManager.Instance.SFX_M_Sleep, transform);
        }
    }
}
