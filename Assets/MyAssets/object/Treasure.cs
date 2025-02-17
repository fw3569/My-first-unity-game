using UnityEngine;

public class Treasure : InteractivityObj
{
  public string item_name;
  public ItemType type;
  public int item_num;
  public Sprite opened_image;
  private static readonly float destroy_time = 2.0f;
  public override string Interactive(GameObject from){
    ItemManager.Instance().Add(type,item_name,item_num);
    interactivity = false;
    GetComponent<SpriteRenderer>().sprite = opened_image;
    GetComponent<AudioSource>().enabled = true;
    GameManager.Instance().AddDeletedObj(transform.GetPath());
    Destroy(gameObject,destroy_time);
    return "Get "+item_name+" x "+item_num;
  }
  public override void ResetInteractive(GameObject from){
  }
}
