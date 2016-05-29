using UnityEngine;
using GameLogic;

public class SceneManagerScript : MonoBehaviour
{
    #region SINGLETON

    // NOTE(Cristian): The SceneManagerScript runs *before* any other script.
    // This means that this way of getting the instance should always work
    private static SceneManagerScript _instance;
    internal static SceneManagerScript GetInstance()
    {
        if (_instance == null)
        {
            // TODO(Cristian): Create instance here? Diagnose?
            string message = "No Scene Manager Found!";
            throw new System.InvalidProgramException(message);
        }

        return _instance;
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        // We set the static instance
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO(Cristian): Should we receive an EntityScript and manage all in unity-speak?
    internal Entity SelectedEntity { get; set; }

    // GUI TEST
    void OnGUI()
    {
        float marginRatio = 0.01f;
        float widthRatio = 0.25f;
        float heightRatio = 0.5f;

        Rect guiRect = new Rect(marginRatio * Screen.width, marginRatio * Screen.height,
                                widthRatio * Screen.width, heightRatio * Screen.height);
        GUI.Box(guiRect, "Debug Window");
    }

}