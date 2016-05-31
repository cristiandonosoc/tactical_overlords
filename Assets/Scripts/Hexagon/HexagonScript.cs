using UnityEngine;
using System.Collections;

public class HexagonScript : MonoBehaviour
{
    #region EDITOR

    public Color PreviousColor;
    public Color CurrentColor;

    #endregion

    private SpriteRenderer _spriteRenderer;


    // Use this for initialization
    void Start ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = CurrentColor;
    }

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

    // NOTHING FOR NOW
    //// Update is called once per frame
    //void Update ()
    //{

    //}
}
