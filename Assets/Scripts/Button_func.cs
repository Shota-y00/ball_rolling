using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_func : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick_game()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void Onclick_menu()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
