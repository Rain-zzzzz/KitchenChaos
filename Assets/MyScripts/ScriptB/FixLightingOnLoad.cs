using UnityEngine;
using UnityEngine.SceneManagement;

public class FixLightingOnLoad : MonoBehaviour
{
    private void OnEnable()
    {
        //刷新全局GI环境
        DynamicGI.UpdateEnvironment();
    }
}