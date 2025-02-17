using UnityEngine;
using UnityEngine.EventSystems;
public class ItemIcon : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
  RectTransform rect_transform;
  Vector2 start_pos;
  public ItemType type = ItemType.ITEM;
  private Vector2 screen_resolution;
  private Vector2 canvas_resolution;
  private Vector2 parent_pos;
  public Item item;
  // show cold time
  public GameObject black_mask;
  public GameObject cold_mask;
  // show amount
  public GameObject text_box;
  public Player player;
  public Equip equip = null;
  public void Awake(){
    rect_transform = GetComponent<RectTransform>();
  }
  public void Start(){
    player = GameManager.Instance().player;
  }
  public void OnEnable(){
    text_box.SetActive(false);
  }
  public void Update(){
    UpdateColdMask();
  }
  public void UpdateColdMask(){
    if(item!=null){
      if(item.skill_cold_resume_time>Time.time){
        black_mask.SetActive(true);
        cold_mask.SetActive(true);
        cold_mask.GetComponent<RectTransform>().localScale = new Vector3(1,Mathf.Min(1,(item.skill_cold_resume_time-Time.time)/item.skill_cold_time));
      } else {
        if(player.magic<item.magic_cost||(item.item_cost!=0&&(player.items[name]<item.item_cost||GameManager.Instance().is_in_boss_room))){
          black_mask.SetActive(true);
        } else {
          black_mask.SetActive(false);
        }
        cold_mask.SetActive(false);
      }
    }
  }

  public virtual void OnDrag(PointerEventData event_data){
    // match width
    rect_transform.anchoredPosition = (event_data.position/screen_resolution-Vector2.one
    /2)*canvas_resolution-parent_pos;
  }

  public virtual void OnBeginDrag(PointerEventData event_data)
  {
    screen_resolution = new Vector2(Screen.width,Screen.height);
    canvas_resolution = new Vector2(1,(float)Screen.height/Screen.width)*CanvasSetting.target_resolution.x;
    parent_pos = transform.parent.GetComponent<RectTransform>().anchoredPosition;
    start_pos = GetComponent<RectTransform>().localPosition;
  }

  public virtual void OnEndDrag(PointerEventData event_data)
  {
    GameObject obj = event_data.pointerCurrentRaycast.gameObject;
    if(obj.CompareTag("ItemSlot")&&type==ItemType.ITEM){
      obj.GetComponent<ItemSlot>().UpdateContent(name);
    } else if(obj.CompareTag("SkillSlot")&&type==ItemType.SKILL){
      obj.GetComponent<SkillSlot>().UpdateContent(name);
    } else if(obj.CompareTag("EquipSlot")&&type==ItemType.EQUIP){
      obj.GetComponent<EquipSlot>().UpdateContent(equip);
    }
    rect_transform.localPosition = start_pos;
    gameObject.SetActive(false);
    gameObject.SetActive(true);
  }
  public virtual void OnPointerEnter(PointerEventData event_data){
    text_box.SetActive(true);
  }
  public virtual void OnPointerExit(PointerEventData event_data){
    text_box.SetActive(false);
  }
  public virtual void OnPointerClick(PointerEventData event_data){
    if(event_data.button==PointerEventData.InputButton.Left){
      if(event_data.clickCount>=2){
        Trigger(player.weapon.transform.parent.position,player.cur_direction,player.weapon.transform.parent.rotation,player,null);
      }
    }
  }
  public virtual bool Trigger(Vector2 pos, Vector2 dir, Quaternion rot, Creature source, GameObject target){
    if(item!=null){
      bool ret = item.Trigger(pos, dir, rot, source, target);
      ItemManager.Instance().UpdateItem();
      return ret;
    } else {
      print("Empty ItemIcon");
    }
    return false;
  }
}
