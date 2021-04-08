using System;
using Mobge.Animation;
using UnityEngine;

public static class ExtendedAnimationCurve 
{
    [Serializable]
    public struct RealtimeAnimationCurve
    {
        public AnimationCurve curve;

        public float Length => _endTime;

        private float _timer;
        private float _endTime;

        public void DetermineCurveEndTime()
        {
            _endTime = curve.keys[curve.keys.Length - 1].time;
        }

        public bool TryMove(float deltaTime)
        {
            if (_timer == _endTime) return false;

            _timer = Mathf.Min(_endTime, _timer + deltaTime);
            return true;
        }

        public float Evaluate()
        {
            return curve.Evaluate(_timer);
        }

        public void Reset()
        {
            _timer = 0;
        }
    }
    
    [Serializable]
    public struct RealtimeCurve
    {
        public Curve curve;

        private float _timer;
        private float _endTime;

        public void Initialize()
        {
            _timer = 0;
            curve.EnsureInit(true);
            var keys = curve.Keys;
            _endTime = keys[keys.Length - 1].time;
        }

        public bool TryMove(float deltaTime, out float value)
        {
            value = 0;
            if (_timer == _endTime) return false;

            _timer = Mathf.Min(_endTime, _timer + deltaTime);
            value = curve.Evaluate(_timer);
            return true;
        }

        public void Reset()
        {
            _timer = 0;
        }
    }
}
