using UnityEngine;

public class Touch_script : MonoBehaviour
{
    private GameObject A;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Frends")
        {
            A = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == A )
        {
            A = null;
        }
    }

    public GameObject Get()
    {
        if (!A){
            return null;
        }
        else return A;
    }
}
