using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillSlot : SkillBox,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
  public string bingding_action_name;
  InputAction binding_action;
  private bool binding_key = false;
  private float binding_key_release_time;
  public Player player;
  public int slot_id;
  public GameObject key_binding_text;
  public Vector2 target_text_size;
  public int target_text_font;
  public int default_text_font;
  public void Awake(){
    binding_action = InputSystem.actions.FindAction(bingding_action_name);
  }

  public void Update(){
    if(player.receive_input){
      float now = Time.time;
      if(binding_action!=null&&binding_action.WasPressedThisFrame()){
        binding_key = true;
        binding_key_release_time = now+Time.fixedDeltaTime;
      } else if (binding_key_release_time<now){
        binding_key = false;
      }
    } else {
      binding_key = false;
    }
  }
  public void FixedUpdate(){
    if(binding_key){
      DoAction();
      binding_key = false;
    }
  }
  public bool DoAction(){
    if(icon!=null){
      return icon.Trigger(player.weaponwrap.transform.position,player.cur_direction,player.weaponwrap.transform.rotation,player,null);
    } else {
      print("Empty SkillSlot");
    }
    return false;
  }
  public void UpdateContent(string new_item_name){
    UpdateItem(new_item_name, false);
    if(icon!=null){
      OnSetItem();
    } else {
      OnRemoveItem();
    }
  }
  public void LoadSlotContent(){
    string item_name = null;
    if(GameManager.global_data.skill_slots!=null&&GameManager.global_data.skill_slots.Count>slot_id){
      item_name = GameManager.global_data.skill_slots[slot_id];
    }
    if(item_name!=null){
      UpdateContent(item_name);
    }
  }

  public virtual void OnPointerEnter(PointerEventData event_data)
  {
    if(icon!=null){
      icon.OnPointerEnter(event_data);
    }
  }
  public virtual void OnPointerExit(PointerEventData event_data)
  {
    if(icon!=null){
      icon.OnPointerExit(event_data);
    }
  }
  public virtual void OnPointerClick(PointerEventData event_data)
  {
    if(icon!=null){
      icon.OnPointerClick(event_data);
    }
    if(event_data.button==PointerEventData.InputButton.Right){
      UpdateContent("");
    }
  }
  public void OnSetItem(){
    key_binding_text.GetComponent<RectTransform>().sizeDelta = target_text_size;
    key_binding_text.GetComponent<Text>().fontSize = target_text_font;
  }
  public void OnRemoveItem(){
    key_binding_text.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
    key_binding_text.GetComponent<Text>().fontSize = default_text_font;
  }
}
