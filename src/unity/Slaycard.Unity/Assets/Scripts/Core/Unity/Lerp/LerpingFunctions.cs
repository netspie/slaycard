﻿using System;
using System.Collections;

namespace Core.Unity.Lerp
{
    public class LerpingFunctions
    {
        public static Func<float, float> SinerpLerp = t => (float) System.Math.Sin(t * System.Math.PI * 0.5f);
        public static Func<float, float> LinearLerp = t => t * t;
        public static Func<float, float> CoserpLerp = t => (float) (1f - System.Math.Cos(t * System.Math.PI * 0.5f));
        public static Func<float, float> ExpLerp = t => t * t;
        public static Func<float, float> SmoothStepLerp = t => t * t * (3f - 2f * t);
        public static Func<float, float> SmootherStepLerp = t => t * t * t * (t * (6f * t - 15f) + 10f);

        public enum LerpFunctionType
        {
            Sinerp,
            CoserpLerp,
            Linear,
            Exp,
            Smooth,
            Smoother
        }

        public static Func<float, float> GetLerpFunction(LerpFunctionType type)
        {
            if (type == LerpFunctionType.Sinerp)
                return LerpingFunctions.SinerpLerp;

            if (type == LerpFunctionType.CoserpLerp)
                return LerpingFunctions.CoserpLerp;

            if (type == LerpFunctionType.Linear)
                return LerpingFunctions.LinearLerp;

            if (type == LerpFunctionType.Exp)
                return LerpingFunctions.ExpLerp;

            if (type == LerpFunctionType.Smooth)
                return LerpingFunctions.SmoothStepLerp;

            if (type == LerpFunctionType.Smoother)
                return LerpingFunctions.SmootherStepLerp;

            return LerpingFunctions.LinearLerp;
        }

        public static TCoroutine Lerp<TValue, TCoroutine>(
            Func<TValue, TValue, float, TValue> lerpFunction,
            Func<TValue> getValue,
            Action<TValue> setValue,
            TValue targetValue,
            float durationSeconds,
            Func<IEnumerator, TCoroutine> startCoroutine,
            Func<float> getDeltaTime,
            Action onDone,
            Action<float> onFrame = null)
        {
            var startValue = getValue();
            var lerpEnumerator = LerpingFunctions.Lerp(
                lerpFunction,
                startValue, targetValue, durationSeconds, setValue, getDeltaTime, onDone);

            return startCoroutine(lerpEnumerator);
        }

        public static TCoroutine Lerp<TValue, TCoroutine>(
            Func<TValue, TValue, float, TValue> lerpFunction,
            Func<TValue> getValue,
            Action<TValue> setValue,
            TValue targetValue,
            float durationSeconds,
            Func<IEnumerator, TCoroutine> startCoroutine,
            Func<float> getDeltaTime,
            Func<float, float> curveLerp,
            Action onDone = null,
            Action<float> onFrame = null)
        {
            var startValue = getValue();
            var lerpEnumerator = LerpingFunctions.Lerp(
                lerpFunction,
                startValue, targetValue, durationSeconds, setValue, getDeltaTime, curveLerp, onDone, onFrame);

            return startCoroutine(lerpEnumerator);
        }
        
        private static IEnumerator Lerp<TValue>(
            Func<TValue, TValue, float, TValue> lerpFunction,
            TValue startValue,
            TValue endValue,
            float duration,
            Action<TValue> setValue,
            Func<float> getTime,
            Action onDone = null,
            Action<float> onFrame = null)
        {
            return Lerp(
                lerpFunction,
                startValue, endValue, duration, setValue, getTime, t => t, onDone, onFrame);
        }

        private static IEnumerator Lerp<TValue>(
            Func<TValue, TValue, float, TValue> lerpFunction,
            TValue startValue, 
            TValue endValue, 
            float duration,
            Action<TValue> setValue, 
            Func<float> getTime,
            Func<float, float> curveLerp,
            Action onDone = null,
            Action<float> onFrame = null)
        {
            float time = 0;

            while (time < duration)
            {
                float t = time / duration;
                t = curveLerp(t);

                var colorLerped = lerpFunction(startValue, endValue, t);
                setValue(colorLerped);

                var currentTime = getTime();
                onFrame?.Invoke(currentTime);
                time += currentTime;

                yield return null;
            }

            setValue(endValue);
            onDone?.Invoke();
        }
    }
}
