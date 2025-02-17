using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
  public Text text;
  public ItemIcon icon;
  public string cur_item_name;
  public Player player;
  public void Awake() {
    foreach(Transform child in transform){
      if(child!=null){
        if(child.name=="Text"){
          text = child.GetComponent<Text>();
          break;
        }
      }
    }
  }
  public void UpdateItem(){
    if(cur_item_name!=null&&cur_item_name!=""){
      text.text = player.items[cur_item_name].ToString();
    }
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
        icon.name = item_name;
        text.text = player.items[item_name].ToString();
      } else {
        text.text = "";
      }
      cur_item_name=item_name;
    } else {
      UpdateItem();
    }
  }
  public void UpdateNum(){
    if(icon!=null){
      text.text = player.items[cur_item_name].ToString();
    }
  }
}
