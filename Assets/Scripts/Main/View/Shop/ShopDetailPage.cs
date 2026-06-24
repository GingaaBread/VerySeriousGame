using Main.Entity;
using Main.Entity.Upgrade;
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

        public void Render(UpgradeSo upgrade)
        {
            _nameText.text = upgrade.UpgradeName;
            _descriptionText.text = upgrade.Description;
            _iconImage.sprite = upgrade.Icon;
        }
    }
}