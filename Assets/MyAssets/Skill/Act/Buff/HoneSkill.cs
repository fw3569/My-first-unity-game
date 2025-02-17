using UnityEngine;

public class HoneSkill : BuffSkill
{
  public override Transform TargetTransform(Creature creature){
    return creature.weapon!=null?creature.weapon.transform:creature.transform;
  }
}
