using System;
using System.Collections;
using UnityEngine;
using static Core.Unity.Lerp.LerpingFunctions;

namespace Core.Unity.Lerp
{
    public class LerpFunctions
    {
        public static Coroutine LerpRotationZ(
            Func<IEnumerator, Coroutine> startCoroutine,
            Transform transform,
            float targetValue,
            float durationSeconds = 0.75f,
            LerpFunctionType type = LerpFunctionType.Smooth,
            Action onDone = null)
        {
            return LerpingFunctions.Lerp(
                Mathf.Lerp,
                UnityGetSetFuncs.GetTransformRotationZFunc(transform),
                UnityGetSetFuncs.SetTransformRotationZAction(transform),
                targetValue,
                durationSeconds,
                startCoroutine,
                GetDeltaTime,
                LerpingFunctions.GetLerpFunction(type),
                onDone);
        }

        public static float GetDeltaTime() => Time.deltaTime;
    }
}
