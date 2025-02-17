using UnityEngine;

public class EquipBox : MonoBehaviour
{
  public ItemIcon icon;
  public string cur_item_name;
  public void UpdateItem(){
  }
  public void UpdateItem(string item_name,bool enabled = true){
    if(cur_item_name!=item_name){
      if(cur_item_name!=null){
        if(icon!=null){
          Destroy(icon.gameObject);
          icon = null;
        }
      }
      if(ItemManager.icons.ContainsKey(item_name)){
        icon = Instantiate(ItemManager.icons[item_name],transform);
        if(icon.item!=null){
          icon.item = ItemManager.items[item_name];
        }
        icon.name =item_name;
      }
      cur_item_name=item_name;
    }
  }
}
