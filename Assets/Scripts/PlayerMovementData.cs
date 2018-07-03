using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "PlayerMovementData", fileName = "PlayerMovementData", order = 3)]
public class PlayerMovementData : ScriptableObject
{
    public float speed;
    public float jumpHeight;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float halfSize;

    public override string ToString()
    {
        return string.Format("[PlayerMovementData: speed={0}, jumpHeight={1}, fallMultiplier={2}, lowJumpMult={3}]",
                            speed, jumpHeight, fallMultiplier, lowJumpMultiplier);
    }
}