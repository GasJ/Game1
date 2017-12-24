using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Utility : MonoBehaviour
{
    public delegate void ClearContentEventHandler();

    /// <summary>
    /// The prefab of dialog
    /// </summary>
    public GameObject dialogPrefab;
    
    public enum Direction
    {
        /// <summary>
        /// Used for default value or stand for not applying
        /// </summary>
        None,
        Left, Right, Up, Down 
    }

    public enum MoveMode
    {
        /// <summary>
        /// Move smoothly to the destination with a constant speed
        /// </summary>
        Normal,
        /// <summary>
        /// Move immediately to the destination 
        /// </summary>
        Teleport
    }

    public enum CharacterType
    {
        Player, AI
    }

    /// <summary>
    /// The declaration of button
    /// </summary>
    public class ButtonDeclaration
    {
        /// <summary>
        /// The text to be shown on the button
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The action to be executed when the button is clicked
        /// </summary>
        public UnityAction OnClickHandler { get; set; }
    }

    /// <summary>
    /// Show a dialog on screen
    /// </summary>
    /// <param name="content">The content to be shown</param>
    /// <param name="buttonDeclaration1">The declaration for the first button, leave null to keep it inactive</param>
    /// <param name="buttonDeclaration2">The declaration for the first button, leave null to keep it inactive</param>
    public static void ShowDialog(string content, ButtonDeclaration buttonDeclaration1, ButtonDeclaration buttonDeclaration2)
    {
        if (!GlobalController.IsShowingDialog)
        {
            var dialog = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Dialog"), GlobalController.CurrentCanvasGameObject.transform);
            var dialogComp = dialog.GetComponent<Dialog>();
            UnityAction destroyDialog = () =>
            {
                Destroy(dialog);
                GlobalController.IsShowingDialog = false;
            };
            dialogComp.transform.Find("TextField").gameObject.GetComponent<Text>().text = content;
            if (buttonDeclaration1 != null)
            {
                dialogComp.transform.Find("Button1").gameObject.SetActive(true);
                var button1 = dialogComp.transform.Find("Button1").gameObject.GetComponent<Button>();
                button1.transform.Find("Text").gameObject.GetComponent<Text>().text = buttonDeclaration1.Text;
                button1.onClick.AddListener(destroyDialog);
                button1.onClick.AddListener(buttonDeclaration1.OnClickHandler);
            }
            if (buttonDeclaration2 != null)
            {
                dialogComp.transform.Find("Button2").gameObject.SetActive(true);
                var button2 = dialogComp.transform.Find("Button2").gameObject.GetComponent<Button>();
                button2.transform.Find("Text").gameObject.GetComponent<Text>().text = buttonDeclaration2.Text;
                button2.onClick.AddListener(destroyDialog);
                button2.onClick.AddListener(buttonDeclaration2.OnClickHandler);
            }
            var cancelButton = dialogComp.transform.Find("CancelButton").gameObject.GetComponent<Button>();
            cancelButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "OK";
            cancelButton.onClick.AddListener(destroyDialog);

            GlobalController.IsShowingDialog = true;
            GlobalController.CurrentCanvasGameObject.transform.Find("EventSystem").gameObject.GetComponent<EventSystem>()
                .SetSelectedGameObject(dialogComp.transform.Find("CancelButton").gameObject);
        }
    }

    /// <summary>
    /// Show a dialog on screen
    /// </summary>
    /// <param name="content">The content to be shown</param>
    /// <param name="cancelButtonAction">The additional action of cancel button</param>
    public static void ShowDialog(string content, UnityAction cancelButtonAction)
    {
        if (!GlobalController.IsShowingDialog)
        {
            var dialog = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Dialog"), GlobalController.CurrentCanvasGameObject.transform);
            var dialogComp = dialog.GetComponent<Dialog>();
            UnityAction destroyDialog = () =>
            {
                Destroy(dialog);
                GlobalController.IsShowingDialog = false;
            };
            dialogComp.transform.Find("TextField").gameObject.GetComponent<Text>().text = content;
            var cancelButton = dialogComp.transform.Find("CancelButton").gameObject.GetComponent<Button>();
            cancelButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "OK";
            cancelButton.onClick.AddListener(destroyDialog);
            cancelButton.onClick.AddListener(cancelButtonAction);

            GlobalController.IsShowingDialog = true;
            GlobalController.CurrentCanvasGameObject.transform.Find("EventSystem").gameObject.GetComponent<EventSystem>()
                .SetSelectedGameObject(dialogComp.transform.Find("CancelButton").gameObject);
        }
    }

    /// <summary>
    /// Show a dialog on screen
    /// </summary>
    /// <param name="content">The content to be shown</param>
    public static GameObject ShowDialog(string content)
    {
        if (!GlobalController.IsShowingDialog)
        {
            var dialog = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Dialog"), GlobalController.CurrentCanvasGameObject.transform);
            var dialogComp = dialog.GetComponent<Dialog>();

            dialogComp.transform.Find("TextField").gameObject.GetComponent<Text>().text = content;
            dialogComp.transform.Find("CancelButton").gameObject.SetActive(false);

            GlobalController.IsShowingDialog = true;

            return dialog;
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// Show a pause menu on screen
    /// </summary>
    public static void ShowPauseMenu()
    {
        if (!GlobalController.IsShowingPauseMenu)
        {
            var pauseMenu = Instantiate
            (
                Resources.Load<GameObject>("Prefabs/UI/PauseMenu"), 
                GlobalController.CurrentCanvasGameObject.transform
            );
            GlobalController.IsShowingPauseMenu = true;
            GlobalController.CurrentCanvasGameObject.transform.Find("EventSystem").gameObject.GetComponent<EventSystem>()
                .SetSelectedGameObject(pauseMenu.transform.Find("ResumeButton").gameObject);
        }
    }
}
