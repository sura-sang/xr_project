using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public class SadSkill : ISkill
    {
        private Player _player;
        private CharacterController _controller;

        public SadSkill(Player player, CharacterController controller)
        {
            _player = player;
            _controller = controller;
        }

        public void OnMove(Vector2 input)
        {
            var dir = _player.InputToCameraSpace(input);

            _player.SmoothRotation(dir);

            dir *= _player.Speed;
            dir.y = _controller.isGrounded ? -1 : _player.MoveDir.y - _player.Gravity * Time.deltaTime;
            _player.MoveDir = dir;
            
            foreach (var eye in _player.SadEyes)
            {
                eye.SetTearLine();
            }
        }

        public void UpdateSkill() { }

        public void FixedUpdateSkill()
        {
            // foreach (var eye in _player.SadEyes)
            // {
            //     eye.SetTearHitPoints();
            // }
        }
        
        public void InitializeSkill()
        {
            foreach (var eye in _player.SadEyes)
            {
                eye.ResetTears();
            }
        }

        public void ClearSkill()
        {
            foreach (var eye in _player.SadEyes)
            {
                eye.ResetTears();
            }
        }


        /*
        [SerializeField] LayerMask _tearBlock;

        [SerializeField] float _speed;
        [SerializeField] float _gravity;
        [SerializeField] float _downGravity;

        [SerializeField] int _sampleCount;
        [SerializeField] int _skipCount;

        [SerializeField] float _accuracy;


        [SerializeField] Transform[] _eyes;
        [SerializeField] LineRenderer[] _lineRenderers;

        [SerializeField] GameObject effect;

        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            if (!_player.IsSkill) SkillEnd();
        }

        public void OnSkill()
        {
            if (_player.IsSkill)
            {
                for (int i = 0; i < _eyes.Length && i < _lineRenderers.Length; i++)
                {
                    SetTearLine(_eyes[i], _lineRenderers[i]);
                    effect.SetActive(true);
                }
            }
        }

        public void SkillEnd()
        {
            for (int i = 0; i < _lineRenderers.Length; i++)
            {
                _lineRenderers[i].positionCount = 0;
            }
            effect.SetActive(false);
        }

        private List<Vector3> GetTearsPositions(Transform eye)
        {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(eye.position);

            var eyeDir = eye.forward * _speed;
            var time = _accuracy;

            for (int i = 1; i < _sampleCount; i++)
            {
                var pos = (eyeDir + (Vector3.down * _gravity * time)) * time;

                time += _accuracy;
                positions.Add(eye.position + pos);
            }

            return positions;
        }

        private (int lastIndex, RaycastHit hit) GetTearHitPoints(List<Vector3> tears, int skip)
        {
            var lastIndex = tears.Count;
            var hit = new RaycastHit();

            for (int i = skip; i < tears.Count; i += skip)
            {
                var dir = tears[i] - tears[i - skip];
                var distance = dir.magnitude;

                if (Physics.Raycast(tears[i - skip], dir, out hit, distance, _tearBlock))
                {
                    return (i, hit);
                }
            }

            return (lastIndex, hit);
        }

        private void SetTearLine(Transform eye, LineRenderer line)
        {
            var positions = GetTearsPositions(eye);
            (var lastIndex, var hitPoint) = GetTearHitPoints(positions, 1);

            if (lastIndex != positions.Count)
            {
                lastIndex++;

                positions.RemoveRange(lastIndex, positions.Count - lastIndex);
                positions[^1] = hitPoint.point;
            }

            line.positionCount = lastIndex;
            line.SetPositions(positions.ToArray());
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _eyes.Length; i++)
            {
                foreach (var point in GetTearsPositions(_eyes[i]))
                {
                    Gizmos.DrawWireSphere(point, 0.1f);
                }
            }
        }
        */
    }
}