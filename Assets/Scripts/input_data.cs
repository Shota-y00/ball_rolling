using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_data : MonoBehaviour
{
    // メインのゲームにもっていく
    public static int w;
    public static int h;
    public static float time;

    public InputField inputField1;
    public InputField inputField2;
    public Slider slider;
    public Text slider_text;
    // Start is called before the first frame update
    void Start()
    {
        inputField1 = inputField1.GetComponent<InputField>();
        inputField2 = inputField2.GetComponent<InputField>();
        slider = slider.GetComponent<Slider>();
        slider_text = slider_text.GetComponent<Text>();
        inputField1.ActivateInputField();
        inputField2.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void getInputtext1()
    {
        int.TryParse(inputField1.text, out w);
    }

    public void getInputtext2()
    {
        int.TryParse(inputField2.text, out h);
    }

    public void getslider()
    {
        time = slider.value;
        setValue();
    }

    private void setValue()
    {
        slider_text.text = "現在の値：" + slider.value;
    }
    
}
