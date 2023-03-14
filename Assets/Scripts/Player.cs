using UnityEngine;

public class Player : MonoBehaviour
{
    public int Rank { get; private set; }

    [SerializeField] private int _minRank;
    [SerializeField] private int _maxRank;

    private void Awake()
    {
        Rank = Random.Range(_minRank, _maxRank);
    }
}