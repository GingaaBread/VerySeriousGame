using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View.Shop
{
    public class RequiredResourceView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _amountText;

        public void Render(Sprite icon, string amountText, Color color)
        {
            _iconImage.sprite = icon;
            _amountText.text = amountText;
            _amountText.color = color;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_amountText.rectTransform);
        }
    }
}