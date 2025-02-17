using UnityEngine;

public class Enemy4 : Creature
{
  public override void AutoActionInternal(){
    if(attackable_targets[1].Contains(hate_target)&&Random.value<0.5){
      SetAction("Attack2");
    } else if(attackable_targets[0].Contains(hate_target)) {
      SetAction("Attack1");
    } else {
      SetAction("Move");
    }
  }
}
