using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utility.Transitions
{
    public class TransitionRegistry : MonoBehaviour
    {
        private async void Awake()
        {
            const string key = "transition";
            var op = Addressables.LoadAssetsAsync<Transition>(key);
            var res = await op.Task;

            TransitionManager.Instance.Register(res.ToArray());
        }
    }
}