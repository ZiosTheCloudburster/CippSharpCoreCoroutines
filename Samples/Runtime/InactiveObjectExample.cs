#if UNITY_EDITOR
using System.Collections;
using UnityEngine;

namespace CippSharp.Core.Coroutines.Samples
{
    public class InactiveObjectExample : MonoBehaviour
    {
        [TextArea(1, 5)]
        public string info = "";

        [Header("Settings:")]
        [Tooltip("Logs every X seconds. If you change this, you have to wait until next log to take effect.")]
        [SerializeField] private float logEvery = 3;
        
        private void Start()
        {
            gameObject.SetActive(false);
            CoroutineUtils.StartTrackedCoroutine(LogCoroutine());
        }

        IEnumerator LogCoroutine()
        {
            
            while (gameObject.activeSelf == false)
            {
                yield return new WaitForSeconds(logEvery);
                Debug.Log($"{nameof(InactiveObjectExample)} {nameof(LogCoroutine)}() says: \"I'm still sleeping!\"", this);
            }
        }
    }
}
#endif
