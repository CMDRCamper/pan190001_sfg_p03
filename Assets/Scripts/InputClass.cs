using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClass
{
    private string inputDir;
    float decayTime;

    public InputClass()
    {
        inputDir = "Neutral";
        decayTime = 16f;
    }
    public InputClass(string inputDirection, float timeToDie)
    {
        inputDir = inputDirection;
        decayTime = timeToDie;
    }


    public void decayInput()
    {
        decayTime--;
     
    }
    public string getInputDir()
    {
        return inputDir;
    }
}
