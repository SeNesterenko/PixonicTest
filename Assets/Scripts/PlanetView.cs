using TMPro;
using UnityEngine;

public class PlanetView : MonoBehaviour
{
    [SerializeField] private Transform _planetTransform;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void Initialize(PlanetModel planetModel, Vector3 newSize, Sprite sprite)
    {
        _text.text = planetModel.Rank.ToString();
        transform.position = planetModel.Position;
        _planetTransform.localScale = newSize;
        _spriteRenderer.sprite = sprite;
    }
}