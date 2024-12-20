using UnityEngine;

public class MenuElement : MonoBehaviour
{
    [field:SerializeField]
    public GameState AssociatedGameState {  get; private set; } 
}
