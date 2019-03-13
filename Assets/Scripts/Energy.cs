using UnityEngine;

// We need to set the Energy to max once the server has started
public abstract class Energy : MonoBehaviour
{

    // We need _current to be sent to the other clients 
    int _current = 0;

    public int current
    {
        get { return Mathf.Min(_current, max); }
        set
        {
            bool emptyBefore = _current == 0;
            _current = Mathf.Clamp(value, 0, max);
            if (_current == 0 && !emptyBefore) OnEmpty();
        }
    }

    public abstract int max { get; }
    public int recoveryTickRate = 1;

    public float Percent()
    {
        return (current != 0 && max != 0) ? (float)current / (float)max : 0;
    }

    // Could make a Recover function based on items used

    // Server function only
    public virtual void OnEmpty() { }
}