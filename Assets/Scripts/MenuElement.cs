using UnityEngine;

/// <summary>
/// Simple base class for denoting a gameobject as a menu for an associated gamestate
/// </summary>
public class MenuElement : MonoBehaviour
{
    [field: SerializeField]
    public GameState AssociatedGameState { get; private set; }
}
