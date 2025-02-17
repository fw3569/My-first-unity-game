
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EquipSlot : EquipBox,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
  public Equip.Part part;
  public string bingding_action_name;
  InputAction binding_action;
  private bool binding_key = false;
  private float binding_key_release_time;
  public Player player;
  public int slot_id;
  public int accessory_id;
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
    print("DoAction "+ bingding_action_name +" "+ cur_item_name);
    return true;
  }
  public void UpdateContent(Equip equip){
    if(equip==null||equip.part != part){
      return;
    }
    player.SetEquip(equip, accessory_id);
    UpdateItem(equip.name, false);
  }
  public void UpdateContent(string new_item_name){
    if(!ItemManager.item_data_dir.ContainsKey(new_item_name)){
      UpdateContent(new Equip(part));
      return;
    }
    ItemData item_data = ItemManager.item_data_dir[new_item_name];
    if(item_data.type != ItemType.EQUIP||item_data.name != new_item_name){
      return;
    }
    UpdateContent(item_data.equip);
  }
  public void LoadSlotContent(){
    string item_name = null;
    if(GameManager.global_data.equip_slots!=null&&GameManager.global_data.equip_slots.Count>slot_id){
      item_name = GameManager.global_data.equip_slots[slot_id];
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
    if(event_data.button==PointerEventData.InputButton.Right){
      UpdateContent("");
    }
  }
}
