using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Web3Unity;
using ZXing;
using ZXing.QrCode;

public class Register : MonoBehaviour
{
  

    protected VisualElement root;
    protected VisualElement imgQrCode;
    protected VisualElement veMetamask;
    protected Button btnCreate;


    void Start()
    {
      

        root = GetComponent<UIDocument>().rootVisualElement;
        btnCreate = root.Q<Button>("btnCreate");
        btnCreate.clicked += BtnCreate_clicked;
    }

    private async void BtnCreate_clicked()
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {

     
    }
}
