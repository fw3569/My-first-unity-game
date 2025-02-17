using UnityEngine;
using System.Collections.Generic;

public class ItemPanel : MonoBehaviour
{
  private int page_num = 0;
  public int row_size = 5;
  public int col_size = 10;
  static  private int page_size =50;
  public GameObject item_box;
  private List<ItemBox>item_boxs = new();
  private bool inited = false;
  public Player player;
  public float box_distance_x = 0.6f;
  public float box_distance_y = 0.6f;
  public Vector2 center = new(0,0);
  public bool to_update = false;
  public GameManager game_manager;
  void Awake()
  {
    page_size = row_size*col_size;
    float x_start = center.x-(col_size-1)/2.0f*box_distance_x;
    float y_start = center.y+(row_size-1)/2.0f*box_distance_y;
    float x,y= y_start;
    page_num = 0;
    for(int i=0;i<row_size;++i){
      x= x_start;
      for(int j=0;j<col_size;++j){
        GameObject new_obj = Instantiate(item_box,transform);
        new_obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x,y);
        x+=box_distance_x;
        ItemBox new_item_box = new_obj.GetComponent<ItemBox>();
        new_item_box.player = player;
        item_boxs.Add(new_item_box);
      }
      y-=box_distance_y;
    }
    inited = true;
    to_update = true;
  }

  public void Update()
  {
    if(to_update){
      to_update = false;
      UpdateItems();
    }
  }
  public void FixedUpdate(){
  }
  public void UpdateItems(){
    if(!inited){
      return;
    }
    int id = 0;
    int box_id = 0;
    foreach(KeyValuePair<string,int> item in player.items){
      if(id>=(page_num+1)*page_size){
        break;
      }
      if(id>=page_num*page_size){
        ItemBox item_box = item_boxs[box_id];
        item_box.UpdateItem(item.Key);
        ++box_id;
      }
      ++id;
    }
  }
  public void PageDown(){
    print("PageDown");
    if((page_num+1)*page_size<player.items.Count){
      ++page_num;
      UpdateItems();
    }
  }
  public void PageUp(){
    print("PageUp");
    if(page_num!=0){
      --page_num;
      UpdateItems();
    }
  }
  public void NextPanel(){
    game_manager.ShowEquipPanel();
  }
  public void LastPanel(){
    game_manager.ShowSkillPanel();
  }
}
