using UnityEngine;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen instance; // Instance of the FadeScreen MonoBehavior

    //Delegate and after fade action method
    public delegate void ActionAferFadeMethod();
    public ActionAferFadeMethod actionAfterFade;

    [SerializeField] private float fadeStep; // How fast the fade is happening
    [SerializeField] private float timeBeforeAction; // How much time do we wait before calling the actionAfterFade method
    private bool callActionAfterFade; // Do we call an action after the fade?

    private float target; // Target value of the fade screen
    private CanvasGroup canvasGroup; 

    private void Awake() {
        instance = this; 
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update() {
        //Fade out
        if (canvasGroup.alpha < target) {
            canvasGroup.alpha += fadeStep;
        }

        //Fade in
        if (canvasGroup.alpha > target) {
            canvasGroup.alpha -= fadeStep;
        }

        //If faded in
        if (canvasGroup.alpha <= 0) {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

            if (callActionAfterFade) {
                StartCoroutine("CallAction");
            }
        }

        //If Faded out
        if (canvasGroup.alpha >= fadeStep) {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        if (canvasGroup.alpha >= 1f && callActionAfterFade) {
            StartCoroutine("CallAction");
        }
    }

    /// <summary>
    /// Fade the screen out, use callAction = true to call an action after the fade
    /// </summary>
    public void FadeOut(bool callAction = false) {
        target = 1;
        callActionAfterFade = callAction;
    }

    /// <summary>
    /// Fade the screen in, use callAction = true to call an action after the fade
    /// </summary>
    public void FadeIn(bool callAction = false) {
        target = 0;
        callActionAfterFade = callAction;
    }

    /// <summary>
    /// Wait before calling an action after fade
    /// </summary>
    private IEnumerator CallAction() {
        yield return new WaitForSeconds(timeBeforeAction);
        actionAfterFade();
    }
}
