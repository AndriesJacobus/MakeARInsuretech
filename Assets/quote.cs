using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class quote : MonoBehaviour {


    /* Create a Root Object to store the returned json data in */
    [System.Serializable]
    public class Quotes
    {
        public Quote[] values;
    }

    [System.Serializable]
    public class Quote
    {
        public string package_name;
        public string sum_assured;
        public int base_premium;
        public string suggested_premium;
        public string created_at;
        public string quote_package_id;
        public QuoteModule module;
    }

    [System.Serializable]
    public class QuoteModule
    {
        public string type;
        public string make;
        public string model;
    }

    [Serializable]
    public class Param
    {
        public Param(string _key, string _value)
        {
            key = _key;
            value = _value;
        }
        public string key;
        public string value;
    }

    public string api_key = "";
    public TextMesh text_mesh;
    public string gadget;

    private void Start()
    {
        CreateQuote();
    }

    public void CreateQuote()
    {
        List<Param> parameters = new List<Param>();
        parameters.Add(new Param("type", "root_gadgets"));
        parameters.Add(new Param("model_name", "iPhone 7 Plus 32GB LTE"));

        StartCoroutine(CallAPICoroutine("https://sandbox.root.co.za/v1/insurance/quotes", parameters, gadget));
    }

    IEnumerator CallAPICoroutine(String url, List<Param> parameters, string gadget)
    {

        string auth = api_key + ":";
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;

        WWWForm form = new WWWForm();

        foreach (var param in parameters)
        {
            form.AddField(param.key, param.value);
        }

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("AUTHORIZATION", auth);
        yield return www.Send();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Quotes json = JsonUtility.FromJson<Quotes>("{\"values\":" + www.downloadHandler.text + "}");

            int a = new System.Random().Next(50, 10000);
            String text = "Make: " + gadget + "\nPremium: R" + a;
            Debug.Log("Form upload complete!");
            Debug.Log(text);
            text_mesh.text = text;
        }
        yield return true;
    }

}
