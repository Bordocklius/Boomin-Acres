using UnityEngine;

public class Logger : MonoBehaviour
{
    //simple component for logging via unityevents
    
    public void LogMessage(string msg)
    {
        Debug.Log(msg);
    }
}
