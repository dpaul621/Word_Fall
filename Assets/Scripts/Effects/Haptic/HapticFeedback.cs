#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public class HapticFeedback : MonoBehaviour
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void TriggerHaptic();
#endif

    public static void Trigger()
    {
#if UNITY_IOS && !UNITY_EDITOR
        TriggerHaptic();
        Debug.Log("Haptic called and it called trigger haptic");
#else
        //Debug.Log("Haptic called — it did not call trigger haptic because not on iOS or in editor.");
#endif
    }
}
