using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private Slider soundMusicSlider;

    private void Awake()
    {
        // 音效Slider数值变化回调
        soundEffectsSlider.onValueChanged.AddListener((float value) =>
        {
            // value 就是滑块的值(0~1)，在这里写设置音效音量逻辑
            Debug.Log($"音效音量：{value}");
            // AudioManager.Instance.SetSoundEffectsVolume(value);
        });

        // 音乐Slider数值变化回调
        soundMusicSlider.onValueChanged.AddListener((float value) =>
        {
            // value 就是滑块的值(0~1)，在这里写设置音乐音量逻辑
            Debug.Log($"音乐音量：{value}");
            // AudioManager.Instance.SetMusicVolume(value);
        });
    }
}