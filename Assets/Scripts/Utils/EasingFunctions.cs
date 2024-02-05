using System;
using UnityEngine;

namespace Utils
{
    public static class EasingFunctions
    {
        public enum EaseType
        {
            OutQuint = 1,
            OutCubic = 2,
            OutCirc = 3,
            OutSine = 4,
            IsInSine = 5,
            Linear = 6
        }
        public static float OutQuint(float t) =>
            1 - Mathf.Pow(1 - t, 5);

        public static float OutCubic(float t) => 
            1 - Mathf.Pow(1 - t, 3);

        public static float OutCirc(float t) => 
            Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));

        public static float OutSine(float t) =>
            Mathf.Sin(t * Mathf.PI / 2);

        public static float IsInSine(float t) =>
          1 - Mathf.Cos(t * Mathf.PI / 2);

        public static float Linear(float t) =>
            t;
        public static float Ease(EaseType easeType, float t) =>
            easeType switch
            {
                EaseType.OutQuint => OutQuint(t),
                EaseType.OutCubic => OutCubic(t),
                EaseType.OutCirc => OutCirc(t),
                EaseType.OutSine => OutSine(t),
                EaseType.IsInSine => IsInSine(t),
                EaseType.Linear => Linear(t),
                _ => throw new ArgumentOutOfRangeException(nameof(easeType), easeType, null)
            };
    }
}
