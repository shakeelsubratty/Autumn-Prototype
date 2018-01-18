using UnityEngine;
using System.Collections;

public class chr_TempInv : MonoBehaviour {

    public int collectedLeaves = 0;

    public void AddLeaves(int leaves)
    {
        collectedLeaves += leaves;
    }
    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 50), "Collected Leaves: " + collectedLeaves);
    }
}
