using System;
using System.Collections;
using System.Collections.Generic;
using Mobge.Animation;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ExtendedAnimation
{
    public static void PlayAnim(this Animator animator, int state, float normalizedTransitionDuration = 0, int layer = 0, float normalizedTime = 0)
    {
        animator.CrossFade(state, normalizedTransitionDuration, layer, normalizedTime);
    }
    
    public static void PlayAnimWithSpeed(this Animator animator, int state, float speed, int speedMultiplierParam, float normalizedTransitionDuration = 0, int layer = 0, float normalizedTime = 0)
    {
        animator.SetFloat(speedMultiplierParam, speed);
        PlayAnim(animator, state, normalizedTransitionDuration, layer, normalizedTime);
    }

    public static void PlayAnimWithLength(this Animator animator, int state, float length, int speedMultiplierParam, float normalizedTransitionDuration = 0, int layer = 0, float normalizedTime = 0)
    {
        PlayAnimWithSpeed(animator, state, 1, speedMultiplierParam, normalizedTransitionDuration, layer, normalizedTime);
        animator.Update(0);
        var animLength = (1 - normalizedTime) * animator.GetCurrentAnimatorStateInfo(layer).length;
        var speed = animLength / length;
        animator.SetFloat(speedMultiplierParam, speed);
    }
    
    [Serializable]
    public class Animation
    {
        public AnimationPlayType animationPlayType = AnimationPlayType.DefaultSpeed;
        public AnimationStateType animationStateType = AnimationStateType.One;
        [AnimatorState] public int state;
        [Range(0, 1)] public float normalizedTransitionDuration = 0;
        public int layer = 0;
        [Range(0, 1)] public float minNormalizedTime = 0;
        [Range(0, 1)] public float maxNormalizedTime = 0;
        
        //All except default speed
        [AnimatorFloatParameter] public int speedMultiplierParam;

        //With speed or scale with speed
        public float speed = 1;

        //With length
        public float length = 1;
        
        //RandomOneFromMultiple
        [AnimatorState] public int[] states;

        public void Play(Animator animator)
        {
            if (animationPlayType == AnimationPlayType.ScaleWithSpeed)
                throw new Exception("Normalized speed should be fed in order to scale the anim.");

            if (animationStateType == AnimationStateType.RandomOneFromMultiple)
            {
                state = GetRandomState();
            }

            var normalizedTime = Random.Range(minNormalizedTime, maxNormalizedTime);
            if (animationPlayType == AnimationPlayType.DefaultSpeed)
            {
                animator.PlayAnim(state, normalizedTransitionDuration, layer, normalizedTime);
            }
            else if (animationPlayType == AnimationPlayType.WithSpeed)
            {
                animator.PlayAnimWithSpeed(state, speed, speedMultiplierParam, normalizedTransitionDuration, layer, normalizedTime);
            }
            else
            {
                animator.PlayAnimWithLength(state, length, speedMultiplierParam, normalizedTransitionDuration, layer, normalizedTime);
            }
        }

        public void Play(Animator animator, float currentSpeed)
        {
            if (animationPlayType != AnimationPlayType.ScaleWithSpeed)
                throw new Exception("Animator type should be " + nameof(AnimationPlayType.ScaleWithSpeed) + " in order to feed animation with normalized speed!" );
            
            if (animationStateType == AnimationStateType.RandomOneFromMultiple)
            {
                state = GetRandomState();
            }
            
            var normalizedTime = Random.Range(minNormalizedTime, maxNormalizedTime);
            animator.PlayAnimWithSpeed(state, speed, speedMultiplierParam, normalizedTransitionDuration, layer, normalizedTime);
            UpdateSpeed(animator, currentSpeed);
        }

        public void UpdateSpeed(Animator animator, float currentSpeed)
        {
            if (animationPlayType != AnimationPlayType.ScaleWithSpeed)
                throw new Exception("Animator type should be " + nameof(AnimationPlayType.ScaleWithSpeed) + " in order to feed animation with normalized speed!" );
            
            animator.SetFloat(speedMultiplierParam, currentSpeed / speed);
        }
        
        private int GetRandomState()
        {
            return states[Random.Range(0, states.Length)];
        }
    }

    public enum AnimationPlayType
    {
        DefaultSpeed,
        WithSpeed,
        WithLength,
        ScaleWithSpeed
    }
    
    public enum AnimationStateType
    {
        One,
        RandomOneFromMultiple
    }
}