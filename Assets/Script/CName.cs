using UnityEngine;
public class CName : MonoBehaviour
{
    public TMPro.TextMeshPro T;
    void Start()
    {
       // remplace T text with company name , product name and version
         T.text = Application.companyName + "\n" + Application.productName + "\n" + "Version " + Application.version;
    }

}
