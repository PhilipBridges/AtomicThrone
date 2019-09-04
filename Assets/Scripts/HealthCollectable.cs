using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    public AudioClip collectedClip;
    private void OnTriggerStay2D(Collider2D other)
    {
        DudeController controller = other.GetComponent<DudeController>();

        if (controller != null)
        {
            if(DudeController.currentHealth < DudeController.maxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
}
