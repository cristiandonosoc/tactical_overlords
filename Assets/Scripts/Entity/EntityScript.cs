using UnityEngine;
using GameLogic;

public class EntityScript : MonoBehaviour
{
    #region EDITOR

    public float speed;

    #endregion EDITOR

    #region INTERFACE

    private Entity _entity;
    internal Entity Entity
    {
        get
        {
            if (_entity == null)
            {
                string message = "CONTEXT: {0}, EntityScript is not associated with a game entity!";
                throw new System.InvalidOperationException(string.Format(message, this));
            }

            return _entity;
        }
        set
        {
            if (_entity != null)
            {
                string message = "CONTEXT: {0}, Attempting to replace an entity in EntityScript";
                throw new System.InvalidOperationException(string.Format(message, this));
            }

            _entity = value;
        }
    }

    #endregion

    private SceneManagerScript _sceneManager;
    // TODO(Cristian): This is temporal! Not true for all entities
    private BarScript _barScript;
    
    // Use this for initialization
    void Start ()
    {
        _sceneManager = SceneManagerScript.GetInstance();
        _barScript = GetComponentInChildren<BarScript>();
    }

    #region MOUSE INTERACTION

    void OnMouseUp()
    {
        _sceneManager.EntityClick(this);
        _barScript.Range -= 0.1f;
    }

    #endregion
}
