using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utility.Audio
{
    public class AudioRegistry : MonoBehaviour
    {
        private async void Awake()
        {
            var op = Addressables.LoadAssetsAsync<AudioWrapper>("audio-wrapper");
            var res = await op.Task;

            AudioManager.Instance.Register(res.ToArray());
        }
    }
}