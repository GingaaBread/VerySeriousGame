using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View.Shop
{
    public class ShopDetailPage : MonoBehaviour
    {
        [Required] [SerializeField] private TMP_Text _nameText;
        [Required] [SerializeField] private TMP_Text _descriptionText;
        [Required] [SerializeField] private Image _iconImage;

        public void Render(string header, string description, Sprite icon)
        {
            _nameText.text = header;
            _descriptionText.text = description;
            _iconImage.enabled = true;
            _iconImage.sprite = icon;
        }

        public void HideContent()
        {
            _nameText.text = string.Empty;
            _descriptionText.text = string.Empty;
            _iconImage.enabled = false;
        }
    }
}