using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Vuforia;
using System.Reflection;
using System;

public class VuforiaCameraIssueFix : MonoBehaviour
{
    void Awake()
    {
        try
        {
            EventInfo evSceneLoaded = typeof(SceneManager).GetEvent("sceneLoaded");
            Type tDelegate = evSceneLoaded.EventHandlerType;

            MethodInfo attachHandler = typeof(VuforiaRuntime).GetMethod("AttachVuforiaToMainCamera", BindingFlags.NonPublic | BindingFlags.Static);

            Delegate d = Delegate.CreateDelegate(tDelegate, attachHandler);
            SceneManager.sceneLoaded -= d as UnityEngine.Events.UnityAction<Scene, LoadSceneMode>;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Cant remove the AttachVuforiaToMainCamera action: " + e.ToString());
        }
    }
}
