using UnityEngine;

public abstract class FSM : MonoBehaviour
{
    public Health health;

    [SerializeField]
    private string _state = "IDLE";

    public string state
    {
        get { return _state; }
        set { _state = value; }
    }

    [HideInInspector]
    protected GameObject target;

    void Start()
    {
        if (health.current == 0) state = "DEAD";
    }

    void Update()
    {
        // potentially some network stuff here like:
        // if (isClient) UpdateClient();
        // if (isServer) _state = UpdateServer();
        // but for now,
        UpdateClient();
        _state = UpdateServer();
    }

    // update for server for states
    protected abstract string UpdateServer();

    // update for clients for client only related things
    protected abstract void UpdateClient();

    public virtual void OnAggro(GameObject go) { }

}