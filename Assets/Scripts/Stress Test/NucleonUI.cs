using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framerater.StressTest
{
    public class NucleonUI : MonoBehaviour
    {
        public void ResetScene()
        {
            SceneManager.LoadScene(0);
        }
    }

}