using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SceneController : MonoBehaviour {

    public static SceneController sceneControl;

    //public float health;
    //public float experience;

    private void Awake() {
        if(sceneControl == null) {
            DontDestroyOnLoad(gameObject);
            try
            {
                LoadScene();
            }
            catch
            {

            }
            sceneControl = this;
        } else if(sceneControl != this) {
            Destroy(gameObject);
        }
    }

    public void NextScene() {
        if ((SceneManager.sceneCountInBuildSettings-1) > SceneManager.GetActiveScene().buildIndex) {
            print("loading " + (SceneManager.GetActiveScene().buildIndex + 1));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            print("This is the last scene");
        }
        
    }

    public void PreviousScene() {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            print("loading " + (SceneManager.GetActiveScene().buildIndex -1 ));
        } else {
            print("This is the first scene");
        }
        
    }
    private void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.fontSize = 56;
        GUI.Label(new Rect(10, 10, 180, 80), "Active scene index : "  + SceneManager.GetActiveScene().buildIndex, style);
    }

    public void SaveScene()
    {
        FileStream file = File.Open(Application.persistentDataPath + "/sceneInfo.dat", FileMode.Create);
        SceneData data = new SceneData();
        data.sceneNumber = SceneManager.GetActiveScene().buildIndex;
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadScene()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.persistentDataPath + "/sceneInfo.dat"))
        {
            throw new Exception("Scene file does not exist");
        }
        FileStream file = File.Open(Application.persistentDataPath + "/sceneInfo.dat", FileMode.Open);
        SceneData data = (SceneData)bf.Deserialize(file);
       // sceneControl = data.sceneNumber;
        SceneManager.LoadScene(data.sceneNumber);
        print("loading " + (data.sceneNumber));
        file.Close();
    }


}

[Serializable]
class SceneData
{
    public int sceneNumber;

}
