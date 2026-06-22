using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    public class RequiredResourceView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _amountText;

        public void Render(Sprite icon, int amount)
        {
            _iconImage.sprite = icon;
            _amountText.text = string.Empty + amount;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_amountText.rectTransform);
        }
    }
}