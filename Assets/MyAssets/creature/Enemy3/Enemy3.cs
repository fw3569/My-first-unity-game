public class Enemy3 : Creature
{
  public override void AutoActionInternal(){
    if(attackable_targets[0].Contains(hate_target)) {
      SetAction("Attack");
    } else {
      SetAction("Move");
    }
  }
}
