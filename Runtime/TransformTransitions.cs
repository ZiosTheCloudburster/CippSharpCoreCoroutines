using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CippSharp.Core.Coroutines
{
    public static class TransformTransitions
    {
        private enum VectorIndex : sbyte
        {
            X = 0,
            Y = 1,
            Z = 2
        }
        
        public enum FloatParameter : sbyte
        {
            None = -1,
            PosX = 0,
            PosY,
            PosZ,
            RotX,
            RotY,
            RotZ,
            ScaleX,
            ScaleY,
            ScaleZ,
        }
        
        public static Coroutine MoveFloatParameter(Transform transform, Space space, sbyte parameterId, float finalValue, 
            float duration, CompletedCallback onComplete = null, Object debugContext = null)
        {
            string logName = debugContext != null ? LogUtils.LogName(debugContext.GetType()) : string.Empty;
            if (transform == null)
            {
                Debug.LogError(logName+$"{nameof(MoveFloatParameter)} {nameof(transform)} is null!", debugContext);
                onComplete?.Invoke(false);
                return null;
            }

            float currentValue = GetFloat(transform, space, parameterId);
            return MoveFloatParameterInternal(transform, space, parameterId, currentValue, finalValue, duration, onComplete, debugContext);
        }

        private static Coroutine MoveFloatParameterInternal(Transform transform, Space space, sbyte parameterId,
            float currentValue, float finalValue, float duration, CompletedCallback onComplete = null, Object debugContext = null)
        {
            return CoroutineUtils.StartCoroutine(MoveFloatParameterCoroutineInternal(transform, space, parameterId, currentValue, finalValue, duration, onComplete, debugContext));
        }

        private static IEnumerator MoveFloatParameterCoroutineInternal(Transform transform, Space space, sbyte parameterId,
            float currentValue, float finalValue, float duration, CompletedCallback onComplete = null, Object debugContext = null)
        {
            string logName = debugContext != null ? LogUtils.LogName(debugContext.GetType()) : string.Empty;
            
            if (duration <= 0.0f)
            {
                if (transform != null)
                {
                    SetFloat(transform, space, parameterId, finalValue);
                }
                onComplete?.Invoke(true);
                yield break;
            }
            
            float rate = 1.0f / duration;
            float t = 0.0f;
            while (t < 1.0f)
            {
                yield return null;
                t += Time.deltaTime * rate;
                float currentLerp = Mathf.Lerp(currentValue, finalValue, t);
                if (transform != null)
                {
                    SetFloat(transform, space, parameterId, currentLerp);
                }
                else
                {
                    Debug.LogError(logName+$"{nameof(MoveFloatParameterCoroutineInternal)} {nameof(transform)} is null! Fallback exit Coroutine.", debugContext);
                    onComplete?.Invoke(false);
                    yield break;
                }
            }
            
            onComplete?.Invoke(true);
            yield break;
        }

        #region Get Float
        
        /// <summary>
        /// Get float parameter from a transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="space"></param>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public static float GetFloat(Transform transform, Space space, sbyte parameterId)
        {
            switch ((FloatParameter)parameterId)
            {
                case FloatParameter.PosX:
                    switch (space)
                    {
                        case Space.World:
                            return transform.position.x;
                        case Space.Self:
                            return transform.localPosition.x;
                    }
                    break;
                case FloatParameter.PosY:
                    switch (space)
                    {
                        case Space.World:
                            return transform.position.y;
                        case Space.Self:
                            return transform.localPosition.y;
                    }
                    break;
                case FloatParameter.PosZ:
                    switch (space)
                    {
                        case Space.World:
                            return transform.position.z;
                        case Space.Self:
                            return transform.localPosition.z;
                    }
                    break;
                case FloatParameter.RotX:
                    switch (space)
                    {
                        case Space.World:
                            return transform.eulerAngles.x;
                        case Space.Self:
                            return transform.localEulerAngles.x;
                    }
                    break;
                case FloatParameter.RotY:
                    switch (space)
                    {
                        case Space.World:
                            return transform.eulerAngles.y;
                        case Space.Self:
                            return transform.localEulerAngles.y;
                    }
                    break;
                case FloatParameter.RotZ:
                    switch (space)
                    {
                        case Space.World:
                            return transform.eulerAngles.z;
                        case Space.Self:
                            return transform.localEulerAngles.z;
                    }
                    break;
                case FloatParameter.ScaleX:
                    switch (space)
                    {
                        case Space.World:
                            return transform.lossyScale.x;
                        case Space.Self:
                            return transform.localScale.x;
                    }
                    break;
                case FloatParameter.ScaleY:
                    switch (space)
                    {
                        case Space.World:
                            return transform.lossyScale.y;
                        case Space.Self:
                            return transform.localScale.y;
                    }
                    break;
                case FloatParameter.ScaleZ:
                    switch (space)
                    {
                        case Space.World:
                            return transform.lossyScale.z;
                        case Space.Self:
                            return transform.localScale.z;
                    }
                    break;
            }

            return 0;
        }
        
        #endregion

        #region Set Float

        /// <summary>
        /// Set float parameter of a transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="space"></param>
        /// <param name="parameterId"></param>
        /// <param name="newValue"></param>
        public static void SetFloat(Transform transform, Space space, sbyte parameterId, float newValue)
        {
            switch ((FloatParameter)parameterId)
            {
                case FloatParameter.PosX:
                    transform.SetTransformPosition(space, VectorIndex.X, newValue);
                    break;
                case FloatParameter.PosY:
                    transform.SetTransformPosition(space, VectorIndex.Y, newValue);
                    break;
                case FloatParameter.PosZ:
                    transform.SetTransformPosition(space, VectorIndex.Z, newValue);
                    break;
                case FloatParameter.RotX:
                    transform.SetTransformEulerAngle(space, VectorIndex.X, newValue);
                    break;
                case FloatParameter.RotY:
                    transform.SetTransformEulerAngle(space, VectorIndex.Y, newValue);
                    break;
                case FloatParameter.RotZ:
                    transform.SetTransformEulerAngle(space, VectorIndex.Z, newValue);
                    break;
                case FloatParameter.ScaleX:
                    transform.SetTransformScale(space, VectorIndex.X, newValue);
                    break;
                case FloatParameter.ScaleY:
                    transform.SetTransformScale(space, VectorIndex.Y, newValue);
                    break;
                case FloatParameter.ScaleZ:
                    transform.SetTransformScale(space, VectorIndex.Z, newValue);
                    break;
            }
        }

        private static void SetTransformPosition(this Transform transform, Space space, VectorIndex index, float newValue)
        {
            switch (space)
            {
                case Space.World:
                    var pos = transform.position;
                    pos[(int)index] = newValue;
                    transform.position = pos;
                    break;
                case Space.Self:
                    var localPos = transform.localPosition;
                    localPos[(int)index] = newValue;
                    transform.localPosition = localPos;
                    break;
            }
        }
        
        private static void SetTransformEulerAngle(this Transform transform, Space space, VectorIndex index, float newValue)
        {
            switch (space)
            {
                case Space.World:
                    var rot = transform.eulerAngles;
                    rot[(int)index] = newValue;
                    transform.eulerAngles = rot;
                    break;
                case Space.Self:
                    var localRot = transform.localEulerAngles;
                    localRot[(int)index] = newValue;
                    transform.localEulerAngles = localRot;
                    break;
            }
        }
        
        private static void SetTransformScale(this Transform transform, Space space, VectorIndex index, float newValue)
        {
            switch (space)
            {
                case Space.World:
                    Transform parent = transform.parent;
                    transform.SetParent(null);
                    var scale = transform.localScale;
                    scale[(int)index] = newValue;
                    transform.localScale = scale;
                    transform.SetParent(parent);
                    break;
                case Space.Self:
                    var localScale = transform.localScale;
                    localScale[(int)index] = newValue;
                    transform.localScale = localScale;
                    break;
            }
        }

        #endregion
    }
}
