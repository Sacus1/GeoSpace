using UnityEngine;
public class CName : MonoBehaviour
{
    public TMPro.TextMeshPro T;
    void Start()
    {
        //Debug.Log("\nInternet : \n" + Application.internetReachability + "\nPlatforme : \n" + Application.platform + "\nLangue : \n" + Application.systemLanguage);
        T.text = Application.companyName + "\n" + Application.productName + "\n" + Application.version;
    }

}
