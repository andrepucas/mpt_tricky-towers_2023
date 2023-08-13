using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Loads or unloads background music from memory, according to music toggle.
/// </summary>
public class MusicLoader : MonoBehaviour
{
    // V A R I A B L E S

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AssetReference _musicReference;

    private AsyncOperationHandle<AudioClip> _opHandle;
    private bool _isPlaying;

    // M E T H O D S

    public void LoadAndPlay()
    {
        if (_isPlaying) return;
        
        _opHandle = Addressables.LoadAssetAsync<AudioClip>(_musicReference);

        _opHandle.Completed += (operation) => 
        {
            _audioSource.clip = operation.Result;
            _audioSource.Play();
            _isPlaying = true;
        };
    }

    public void StopAndUnload()
    {
        if (!_isPlaying) return;

        _audioSource.Stop();
        _audioSource.clip = null;
        _isPlaying = false;

        if (_opHandle.IsValid()) Addressables.Release(_opHandle);
    }
}
