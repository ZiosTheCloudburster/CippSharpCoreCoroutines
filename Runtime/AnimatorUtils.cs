using System.Collections;
using UnityEngine;

namespace CippSharp.Core.Coroutines
{
    public static class AnimatorUtils
    {
        /// <summary>
        /// Retrieve a more contextual name for logs, based on type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string LogName(System.Type type)
        {
            return $"[{type.Name}]: ";
        }
        
        #region Edit Parameters
        
        /// <summary>
        /// Move an animator float parameter from his current value to final value
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterId"></param>
        /// <param name="finalValue"></param>
        /// <param name="duration"></param>
        /// <param name="onComplete"></param>
        /// <param name="debugContext"></param>
        public static Coroutine MoveFloatParameter(Animator animator, int parameterId, float finalValue, float duration, CompletedCallback onComplete = null, Object debugContext = null)
        {
            string logName = debugContext != null ? LogName(debugContext.GetType()) : string.Empty;
            if (animator == null)
            {
                Debug.LogError(logName+$"{nameof(MoveFloatParameter)} {nameof(animator)} is null!", debugContext);
                onComplete?.Invoke(false);
                return null;
            }

            float target = animator.GetFloat(parameterId);
            return MoveFloatParameterInternal(animator, parameterId, target, finalValue, duration, onComplete, debugContext);
        }

        /// <summary>
        /// Move an animator float parameter from his current value to final value
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterId"></param>
        /// <param name="currentValue"></param>
        /// <param name="finalValue"></param>
        /// <param name="duration"></param>
        /// <param name="onComplete"></param>
        /// <param name="debugContext"></param>
        private static Coroutine MoveFloatParameterInternal (Animator animator, int parameterId, float currentValue, float finalValue, float duration, CompletedCallback onComplete = null, Object debugContext = null)
        {
            return CoroutineUtils.StartCoroutine(MoveFloatParameterCoroutineInternal(animator, parameterId, currentValue, finalValue, duration, onComplete, debugContext));
        }

        private static IEnumerator MoveFloatParameterCoroutineInternal(Animator animator, int parameterId, float currentValue, float finalValue, float duration, CompletedCallback onComplete = null, Object debugContext = null)
        {
            string logName = debugContext != null ? LogName(debugContext.GetType()) : string.Empty;
            
            if (duration <= 0.0f)
            {
                if (animator != null)
                {
                    animator.SetFloat(parameterId, finalValue);
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
                if (animator != null)
                {
                    animator.SetFloat(parameterId, currentLerp);
                }
                else
                {
                    Debug.LogError(logName+$"{nameof(MoveFloatParameterCoroutineInternal)} {nameof(animator)} is null! Fallback exit Coroutine.", debugContext);
                    onComplete?.Invoke(false);
                    yield break;
                }
            }
            
            onComplete?.Invoke(true);
            yield break;
        }

        #endregion
    }
}
