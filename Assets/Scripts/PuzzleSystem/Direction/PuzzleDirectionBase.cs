using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class PuzzleContextDirection : PuzzleContext
    {
        public Vector3 Dir { get; private set; }
        public CharacterMove Character { get; private set; }

        public Player Player { get; private set; }

        public PuzzleContextDirection(Vector3 dir, CharacterMove character, Emotion emotion, Player player) : base(emotion)
        {
            Dir = dir;
            Character = character;
            Player = player;
        }
    }


    public abstract class PuzzleDirectionBase : PuzzleElements
    {
        public int DirCount => _dirCount;
        [SerializeField] private int _dirCount = 4;

        public float StartAngle => _startAngle;
        [SerializeField] private float _startAngle = 0;

        protected PuzzleContextDirection _context;


        public override void OnNotify(PuzzleContext context)
        {
            _context = context as PuzzleContextDirection;
        }

        protected Vector3 GetVector(float radians)
        {
            return new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        }

        protected float GetAngle(Vector3 start, Vector3 end)
        {
            Vector3 dir = end - start;
            dir.y = 0;
            return GetAngle(dir);
        }

        protected float GetAngle(Vector3 dir)
        {
            return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        }

        protected float GetNearestAngle(float angle)
        {
            var plusAngle = 360f / DirCount;

            if (angle < 0)
            {
                angle += 360f;
            }

            for (int i = 0; i < DirCount; i++)
            {
                var result = StartAngle + i * plusAngle;
                if (Mathf.Abs(result - angle) < plusAngle * 0.5f)
                {
                    return result;
                }
            }

            return StartAngle;
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var plusAngle = 360f / DirCount;

            for (int i = 0; i < DirCount; i++)
            {
                var dir = GetVector((StartAngle + i * plusAngle) * Mathf.Deg2Rad).normalized * 3;
                Gizmos.DrawLine(transform.position, transform.position + dir);
            }
        }
    }
}
