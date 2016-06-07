using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    #region EDITOR

    [Range(0, 1)]
    public float EditorRange = 1;

    // OnValidate is called on editor only
    // TODO(Cristian): This is too cumbersome. Create a custom editor
    // so that all properties are tied together.
    void OnValidate()
    {
        if (_barRectTransform == null) { SetupBar(); }
        Range = EditorRange;
    }

    #endregion

    private float _range;
    public float Range
    {
        get { return _range; }
        set
        {
            if ((value < 0) || (value > 1)) { return; }
            _range = value;
            ScaleBar();
        }
    }

    private Color _borderColor;
    public Color BorderColor
    {
        get { return _borderColor; }
        set
        {
            _borderColor = value;
            _borderImage.color = _borderColor;
        }
    }

    private Color _backgroundColor;
    public Color BackgroundColor
    {
        get { return _backgroundColor; }
        set
        {
            _backgroundColor = value;
            _backgroundImage.color = _borderColor;
        }
    }

    private Color _barColor;
    public Color BarColor
    {
        get { return _barColor; }
        set
        {
            _barColor = value;
            _barImage.color = _borderColor;
        }
    }

    RectTransform _barRectTransform;
    Image _borderImage;
    Image _backgroundImage;
    Image _barImage;

    // Use this for initialization
    void Start ()
    {
        SetupBar();
        LoadInitialValues();
    }

    void LoadInitialValues()
    {
        Range = EditorRange;
        _borderColor = _borderImage.color;
        _backgroundColor = _backgroundImage.color;
        _barColor = _barImage.color;
    }

    void SetupBar()
    {
        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "Border":
                    _borderImage = child.GetComponent<Image>();
                    break;
                case "Background":
                    _backgroundImage = child.GetComponent<Image>();
                    break;
                case "Bar":
                    _barRectTransform = child.GetComponent<RectTransform>();
                    _barImage = child.GetComponent<Image>();
                    break;
                default:
                    throw new System.Exception("Falldown to default case");
            }

        }
        _barRectTransform = transform.FindChild("Bar").GetComponent<RectTransform>();
    }

    void ScaleBar()
    {
        // TODO(Cristian): Do we really need to allocate?
        _barRectTransform.localScale = new Vector3(Range, 1, 1);
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
