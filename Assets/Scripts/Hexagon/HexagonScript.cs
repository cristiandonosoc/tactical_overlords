using UnityEngine;
using System.Collections;
using GameLogic;

public class HexagonScript : MonoBehaviour
{
    #region EDITOR

    public Color PreviousColor;
    public Color CurrentColor;

    #endregion

    #region INTERFACE

    private Hexagon _hexagon;
    internal Hexagon Hexagon
    {
        get
        {
            if (_hexagon == null)
            {
                string message = "CONTEXT: {0}, HexagonScript is not associated with a game hexagon!";
                throw new System.InvalidOperationException(string.Format(message, this));
            }
            return _hexagon;
        }
        set
        {
            if (_hexagon != null)
            {
                string message = "CONTEXT: {0}, Attempting to replace an hexagon in HexagonScript";
                throw new System.InvalidOperationException(string.Format(message, this));
            }
            _hexagon = value;

        }

    }

    #endregion INTERFACE

    private SpriteRenderer _spriteRenderer;
    private SceneManagerScript _sceneManager;


    // Use this for initialization
    void Start ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = CurrentColor;

        _sceneManager = SceneManagerScript.GetInstance();
    }

    #region COLOR MANAGEMENT (TO BE REMOVED)

    internal void ChangeColor(Color color)
    {
        PreviousColor = CurrentColor;
        CurrentColor = color;
        _spriteRenderer.color = CurrentColor;
    }

    internal void ChangeColor(Color color, Color previousColor)
    {
        PreviousColor = previousColor;
        CurrentColor = color;
        _spriteRenderer.color = CurrentColor;
    }

    internal void RevertColor()
    {
        // We switch the colors
        Color temp = CurrentColor;
        CurrentColor = PreviousColor;
        PreviousColor = temp;
        _spriteRenderer.color = CurrentColor;
    }

    #endregion COLOR MANAGEMENT (TO BE REMOVED)

    #region MOUSE INTERACTION

    void OnMouseUp()
    {
        _sceneManager.HexagonClick(this);
    }

    #endregion MOUSE INTERACTION


}
