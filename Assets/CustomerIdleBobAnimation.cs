using Unity.Mathematics;
using UnityEngine;

public class CustomerIdleBobAnimation : MonoBehaviour
{
    // This script basically animates the boba shop NPO's bobbing visuals.

    // The multplier speed that this character bobs up/down. For each occilation takes exactly 1(s).
    float characterBobingSpeedMultiplier = 2f;
    float bobbingTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,(7*math.sin(bobbingTimer*math.PI*characterBobingSpeedMultiplier))-55,this.gameObject.transform.localPosition.z);
        bobbingTimer += Time.deltaTime;
    }
}
