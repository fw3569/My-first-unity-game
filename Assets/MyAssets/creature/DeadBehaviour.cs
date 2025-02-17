using UnityEngine;

public class DeadBehaviour : StateMachineBehaviour
{
  public GameObject dead_title;
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
    if (animator.TryGetComponent<Player>(out var player)){
      player.receive_input = false;
    }
    if(dead_title!=null){
      Instantiate(dead_title,GameManager.Instance().main_canvas.transform);
    }
  }
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
    if(animator.TryGetComponent<Player>(out var player)){
      // avoiding exposed map info when resetting position
      player.eye.gameObject.SetActive(false);
      player.Reset();
      GameManager.Instance().DeadReload();
      return;
    }
    Destroy(animator.gameObject);
  }
}
