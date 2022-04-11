#if UNITY_EDITOR
using System;
using System.Collections;
using UnityEngine;

namespace CippSharp.Core.Coroutines.Samples
{
#pragma warning disable 649
   internal class ChangeColorExample : MonoBehaviour
   {
      #region Custom RendererContainer
         
      [Serializable]
      public struct RendererContainer
      {
         public Renderer renderer;

         /// <summary>
         /// Method in a struct that starts a coroutine
         /// </summary>
         /// <param name="time"></param>
         /// <param name="newColor"></param>
         public void ChangeColorAfterTime(float time, Color newColor)
         {
            CoroutineUtils.StartTrackedCoroutine(ChangeColorCoroutine(new WaitForSeconds(time), newColor), OnCoroutineEndCallback);
         }

         private IEnumerator ChangeColorCoroutine(WaitForSeconds wait, Color newColor)
         {
            yield return wait;
            if (renderer == null)
            {
               Debug.LogError("Renderer is null.");
               yield break;
            }

            Material m = renderer.material;
            if (m == null)
            {
               Debug.LogError("Renderer's material is null.");
               yield break;
            }
            
            m.color = newColor;
            yield break;
         }
         
         private void OnCoroutineEndCallback()
         {
            Debug.Log("Color changed");
         }
      }
      
      #endregion
      
      [TextArea(1, 5)]
      public string info = "";
      
      [Header("Settings:")]
      [SerializeField] private float changeColorDelay = 5;
      [SerializeField] private Color newColor = Color.red;
      [Header("References:")]
      [SerializeField] private RendererContainer container = new RendererContainer();
      
      private void Start()
      {
         container.ChangeColorAfterTime(changeColorDelay, newColor);
      }
   }
#pragma warning restore 649
}
#endif