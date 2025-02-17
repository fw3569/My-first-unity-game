using UnityEngine;

public class DropItem : MonoBehaviour
{
  public string item_name = "None";
  public int number = 1;
  public ItemType type = ItemType.ITEM;
  public void OnTriggerExit2D(Collider2D col){
    if(col.name== "ActiveArea"){
      Destroy(gameObject);
    }
  }
}
