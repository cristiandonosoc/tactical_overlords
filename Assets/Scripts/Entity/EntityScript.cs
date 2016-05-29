using UnityEngine;
using GameLogic;

public class EntityScript : MonoBehaviour
{

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
    
    // Use this for initialization
    void Start ()
    {
        _sceneManager = SceneManagerScript.GetInstance();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    #region MOUSE INTERACTION

    void OnMouseUp()
    {
        _sceneManager.SetSelectedEntity(Entity);
    }

    #endregion
}
