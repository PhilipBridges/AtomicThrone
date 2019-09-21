using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public AnimationCurve dropCurve;
    public float dropVal = 0;

    public GameObject shotgun;
    public GameObject magnum;
    public GameObject launcher;
    public GameObject bouncer;

    public void DropShotgunAmmo(Vector2 position)
    {
       GameObject shotgunAmmoDrop = Instantiate(shotgun, position, Quaternion.identity);
    }
    public void DropMagnumAmmo(Vector2 position)
    {
        GameObject magnumAmmoDrop = Instantiate(magnum, position, Quaternion.identity);
    }
    public void DropLauncherAmmo(Vector2 position)
    {
        GameObject launcherAmmoDrop = Instantiate(launcher, position, Quaternion.identity);
    }
    public void DropBouncerAmmo(Vector2 position)
    {
        GameObject bouncerAmmoDrop = Instantiate(bouncer, position, Quaternion.identity);
    }

    public float DropRoll()
    {
        float willDrop = Random.Range(1f, 100f);

        if (willDrop < 35)
        {
            dropVal = CurveWeightedRandom(dropCurve);
            return dropVal;
        }
        return 105;
    }

    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value) * 100;
    }
}
