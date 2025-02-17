using UnityEngine;

public class InteractivityObj : MonoBehaviour
{
  public string text = "Interactive";
  public bool interactivity = true;
  public virtual void Awake(){
    int sorting_order = (int)(transform.position.y * -10.0f);
    Renderer sprite_renderer = GetComponent<Renderer>();
    sprite_renderer.sortingOrder = sorting_order;
  }
  public virtual string Interactive(GameObject from){
    print("Empty Interactive");
    return "Failed";
  }
  public virtual void ResetInteractive(GameObject from){
    // dont reset dead obj
    print("Empty ResetInteractive");
  }
}
