using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Utility.Audio
{
    [CreateAssetMenu(menuName = "Audio Wrapper", fileName = "Audio Wrapper", order = 0)]
    public class AudioWrapper : ScriptableObject
    {
        [field: InfoBox("The id of the wrapper is identical to the file name and must be unique!")]
        [field: ReadOnly]
        [field: SerializeField] public string Id { get; private set; }

        [field: InfoBox("If enabled, the wrapper is activated when registered")]
        [field: SerializeField]
        public bool AutoActivate { get; private set; }

        [field: InfoBox("Defines any number of variables used by the wrapper and their initial values")]
        [field: SerializeField]
        public AudioVariable[] Variables { get; private set; }

        [field: AllowNesting]
        [field: InfoBox("The main sheet used by the wrapper.")]
        [field: SerializeField] public AudioSheet MainSheet { get; private set; }

        [field: AllowNesting]
        [field: InfoBox("Any number of additional sheets that this wrapper uses.")]
        [field: SerializeField] public AudioSheet[] AdditionalSheets { get; private set; }

        private void OnValidate()
        {
            Id = name;
            MainSheet.Validate();

            ValidateSheet(MainSheet);
            foreach (var additionalSheet in AdditionalSheets)
            {
                additionalSheet.Validate();
                ValidateSheet(additionalSheet);
            }

            foreach (IValidated audioVariable in Variables)
            {
                audioVariable.Validate();
            }
        }

        private void ValidateSheet(AudioSheet sheet)
        {
            foreach (var transition in sheet.Transitions)
            {
                var variableExists = Variables.Any(v => v.Id == transition.If.VariableId);
                if (variableExists) continue;

                Debug.LogError($"Transition '{transition.Id}' of sheet '{sheet.Id}' uses a " +
                               $"variable id that does not exist on the wrapper (variable: '{transition.If}')");
            }
        }

        public override string ToString() => Id;

        public bool TryGetSheet(string sheetName, out AudioSheet sheet)
        {
            sheet = null;

            if (MainSheet.Id == sheetName)
            {
                sheet = MainSheet;
                return true;
            }

            var res = AdditionalSheets.FirstOrDefault(sheet => sheet.Id == sheetName);
            if (res == null) return false;

            sheet = res;
            return true;
        }
    }
}