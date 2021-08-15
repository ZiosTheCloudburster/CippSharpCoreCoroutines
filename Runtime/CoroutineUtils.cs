using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CippSharp.Core.Coroutines
{
    /// <summary>
    /// This class allows to runs coroutines from a mono-behaviour external to any context.
    /// </summary>
    public static class CoroutineUtils
    {
        #region Coroutines Container Behaviour
        
        [AddComponentMenu("")]
        [DisallowMultipleComponent]
        private class CoroutinesContainer : MonoBehaviour
        {
#if UNITY_EDITOR
            /// <summary>
            /// Active Coroutines Count
            /// It works until every coroutine is correctly cached for a 'start' and a 'stop' (if they needs it) 
            /// </summary>
            [Header("Infos:")]
            [Tooltip("During editor only it displays the active tracked coroutines count.")]
            [SerializeField] internal int activeTrackedCoroutinesCount = 0;
#endif
        }
        
        #endregion

        #region Container

        /// <summary>
        /// Backing field of Container
        /// </summary>
        private static CoroutinesContainer m_container = null;

        /// <summary>
        /// Retrieve the container to run coroutines
        /// </summary>
        private static CoroutinesContainer Container
        {
            get
            {
                if (m_container != null)
                {
                    return m_container;
                }

                Initialize();
                return m_container;
            }
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            m_container = new GameObject(nameof(CoroutineUtils)+":"+nameof(CoroutinesContainer), new Type[] {typeof(CoroutinesContainer)}).GetComponent<CoroutinesContainer>();
            Object.DontDestroyOnLoad(m_container.gameObject);
        }
        
        #endregion

        /// <summary>
        /// A static instance for wait for end of frame
        /// </summary>
        public static WaitForEndOfFrame WaitForEndOfFrame { get; private set; } = new WaitForEndOfFrame();

        /// <summary>
        /// Null means wait update!
        /// </summary>
        public static YieldInstruction WaitUpdate { get; private set; } = null;
        
        /// <summary>
        /// A static instance for wait for fixed update
        /// </summary>
        public static WaitForFixedUpdate WaitForFixedUpdate { get; private set; } = new WaitForFixedUpdate();
        
        /// <summary>
        /// It starts target coroutine.
        /// </summary>
        /// <param name="enumerator"></param>
        public static Coroutine StartCoroutine(IEnumerator enumerator)
        {
            CoroutinesContainer container = Container;
            return container.StartCoroutine(enumerator);
        }

        #region Start Tracked
        
        /// <summary>
        /// It starts target coroutine and checks for his end
        /// (mostly for debug purpose).
        ///
        /// NOTE: It is strongly suggested, if everything works fine and you no longer want to debug your coroutines, to use <see cref="StartCoroutine"/> instead. 
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="onCoroutineEndCallback"></param>
        /// <returns>WARNING: the return parameter is the 'tracking coroutine' not the one you're starting</returns>
        public static Coroutine StartTrackedCoroutine(IEnumerator enumerator, Action onCoroutineEndCallback = null)
        {
            CoroutinesContainer container = Container;
            return container.StartCoroutine(TrackCoroutine(enumerator, onCoroutineEndCallback));
        }
        
        /// <summary>
        /// Wait for the end of the given coroutine 
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="onCoroutineEndCallback"></param>
        /// <returns></returns>
        private static IEnumerator TrackCoroutine(IEnumerator enumerator, Action onCoroutineEndCallback = null)
        {
            CoroutinesContainer container = Container;
#if UNITY_EDITOR
            container.activeTrackedCoroutinesCount++;
#endif
            yield return container.StartCoroutine(enumerator);
#if UNITY_EDITOR
            container.activeTrackedCoroutinesCount--;
#endif
            onCoroutineEndCallback?.Invoke();
        }
        
        #endregion
        
        /// <summary>
        /// It stops target coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        public static void StopCoroutine(IEnumerator coroutine)
        {
            CoroutinesContainer container = Container;
            container.StopCoroutine(coroutine);
        }

        /// <summary>
        /// It stops target coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        public static void StopCoroutine(Coroutine coroutine)
        {
            CoroutinesContainer container = Container;
            container.StopCoroutine(coroutine);
        }

        /// <summary>
        /// It stops all coroutines that are running in container.
        /// </summary>
        public static void StopAllCoroutines()
        {
            CoroutinesContainer container = Container;
            container.StopAllCoroutines();
        }
    }
}
