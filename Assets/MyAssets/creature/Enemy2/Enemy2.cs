public class Enemy2 : Creature
{
  public Item fireball_item_prefab;
  protected Item fireball_item;
  public override void Awake()
  {
    base.Awake();
    fireball_item = Instantiate(fireball_item_prefab,instanced_prefabs.transform);
  }
  public override int GetMoveAttackAreaId(){
    if(magic>=fireball_item.magic_cost){
      return 1;
    } else {
      return 0;
    }
  }
  public override void AutoActionInternal(){
    if(attackable_targets[1].Contains(hate_target)&&magic>=fireball_item.magic_cost){
      fireball_item.Trigger(weaponwrap.transform.position,cur_direction,weaponwrap.transform.rotation,this,null);
    } else if(attackable_targets[0].Contains(hate_target)){
      SetAction("Attack");
    } else {
      SetAction("Move");
    }
  }
}
