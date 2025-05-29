using Unity.VisualScripting;
using UnityEngine;

public class BeanbagLevelInteractions : ObjectLevelInteractions
{
    protected override void OnTriggerExit(Collider trigger)
    {
        base.OnTriggerExit(trigger);
        //if the object is fully within a hole, play the beanbag sound
        if (trigger.gameObject.CompareTag("HoleActivateTrigger"))
        {
            SoundManager.Instance.PlaySound(10, 28, 0.7f, 1f);
        }
    }
}
