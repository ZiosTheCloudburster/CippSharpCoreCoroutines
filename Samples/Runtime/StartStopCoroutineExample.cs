#if UNITY_EDITOR
#if UNITY_EDITOR
using System;
#endif
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CippSharp.Core.Coroutines.Samples
{
    /// <summary>
    /// Notes:
    /// There are 'two' levels of #IF UNITY_EDITOR only to present
    /// an example where to #if#def correctly 'the editor part' if
    /// this wasn't an example.
    /// </summary>
#pragma warning disable 414
    public class StartStopCoroutineExample : MonoBehaviour
    {
        [TextArea(1, 5)]
        public string info = "";

        [Header("Settings:")] 
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float warpY = 3;

        [Header("Commands:")]
        [SerializeField] private bool stopCoroutine = false;
        [SerializeField] private bool startCoroutine = false;
        
        private bool isRunningCoroutine = false;

        /// <summary>
        /// Saved initial position
        /// </summary>
        private Vector3 initialPosition = Vector3.zero;
        /// <summary>
        /// Cache of your custom running enumerator as coroutine
        /// </summary>
        private IEnumerator customUpdateCoroutine = null;
        
        //Another way could be
        //private Coroutine customUpdateCoroutine;
        //by saving the output of CoroutineUtils.StartCoroutine (instead of tracked one)

        private void OnValidate()
        {
            //clamp speed on assignment... 
            if (speed < 0)
            {
                speed = 0;
            } 
        }

        private void Start()
        {
            initialPosition = transform.position;
            customUpdateCoroutine = CustomUpdateCoroutine();
        }

        private void StartCustomUpdate()
        {
            if (isRunningCoroutine != false)
            {
                return;
            }
            
            isRunningCoroutine = true;
            CoroutineUtils.StartTrackedCoroutine(customUpdateCoroutine);
        }

        private void StopCustomUpdate()
        {
            isRunningCoroutine = false;
        }

        IEnumerator CustomUpdateCoroutine ()
        {
            //Cache transform
            Transform t = transform;
            
            Vector3 currentPosition;
            while (isRunningCoroutine)
            {
                //Prevent null reference: T
                //his may happens sometimes during OnDestroy if coroutine is still running! 
                if (t == null)
                {
                    yield break;
                }
                
                //first frame doesn't count
                yield return null;
               
                //edit current position
                currentPosition = t.position;
                currentPosition += (Vector3.up * speed * Time.deltaTime);
               
                //teleport object down
                if (currentPosition.y > warpY)
                {
                    currentPosition = initialPosition;
                }
                t.position = currentPosition;
            }
            
            Debug.Log($"{nameof(StartStopCoroutineExample)} {nameof(CustomUpdateCoroutine)}() says: \"Coroutine has ended!\"", this);
            yield break;
        }

        #region Custom Editor
        /// <summary>
        /// For practical reasons I usually put custom editors inside
        /// classes that needs them (well #if#deffed) so I can access to
        /// anything of the class that I need to customize 
        /// </summary>
#if UNITY_EDITOR
        [CustomEditor(typeof(StartStopCoroutineExample))]
        private class StartStopCoroutineExampleEditor : Editor
        {
            private static readonly GUILayoutOption[] Empty = new GUILayoutOption[0];
            private StartStopCoroutineExample startStopCoroutineExample = null;
            
            private void OnEnable()
            {
                startStopCoroutineExample = (StartStopCoroutineExample)target;
            }

            public override void OnInspectorGUI()
            {
                DrawInspectorByIterator(serializedObject, DrawPropertyDelegate);
            }
            
            /// <summary>
            /// Same of unity draw editor but with delegate to override ONLY the 1-2 properties that needs customization
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="drawPropertyDelegate"></param>
            private static void DrawInspectorByIterator(SerializedObject obj, Action<SerializedProperty> drawPropertyDelegate)
            {
                obj.UpdateIfRequiredOrScript();
                SerializedProperty iterator = obj.GetIterator();
                for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        drawPropertyDelegate.Invoke(iterator);
                    }
                }
                obj.ApplyModifiedProperties();
            }
            
            /// <summary>
            /// Called once for each serializedProperty in the iterator.
            /// </summary>
            /// <param name="property"></param>
            private void DrawPropertyDelegate(SerializedProperty property)
            {
                ////Draw Stop Coroutine Button
                if (property.name == nameof(stopCoroutine))
                {
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField("Commands:", EditorStyles.boldLabel);
                    if (startStopCoroutineExample.isRunningCoroutine == true)
                    {
                        bool guiStatus = GUI.enabled;
                        GUI.enabled = Application.isPlaying;
                        if (GUILayout.Button(property.displayName, EditorStyles.miniButton, Empty))
                        {
                            startStopCoroutineExample.StopCustomUpdate();
                        }
                        GUI.enabled = guiStatus;
                    }
                }
                //Draw Start Coroutine Button
                else if (property.name == nameof(startCoroutine))
                {
                    if (startStopCoroutineExample.isRunningCoroutine == false)
                    {
                        bool guiStatus = GUI.enabled;
                        GUI.enabled = Application.isPlaying;
                        if (GUILayout.Button(property.displayName, EditorStyles.miniButton, Empty))
                        {
                            startStopCoroutineExample.StartCustomUpdate();
                        }
                        GUI.enabled = guiStatus;    
                    }
                }
                //Draws other properties normally
                else
                {
                    EditorGUILayout.PropertyField(property, true, Empty);
                }
            }
        }
#endif
        #endregion
    }
#pragma warning restore 414
}
#endif
