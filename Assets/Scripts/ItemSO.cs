using UnityEngine;


[CreateAssetMenu(fileName ="Thing", menuName ="Hittable Things")]
public class ItemSO : ScriptableObject
{
    public int PointsIfHit = 5;
    public int TimeToAdd = 5;
}
