using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayFabStudy
{
    public class Intro : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}