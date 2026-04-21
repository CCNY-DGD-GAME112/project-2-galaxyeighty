using UnityEngine;
using TMPro;

public class CollectItem : MonoBehaviour
{
    private int Vial = 0;
    public TextMeshProUGUI vialText;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Vial")
        {
            Vial++;
            vialText.text = "Vials: " + Vial + " / 9".ToString();
            Debug.Log(Vial);
            Destroy(other.gameObject);
        }
    }

}
