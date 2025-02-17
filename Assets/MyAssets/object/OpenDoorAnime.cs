using UnityEngine;

public class OpenDoorAnime : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.transform.parent.gameObject.GetComponent<Door>().DestroyDoor();
    }
}
