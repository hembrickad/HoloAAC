using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMovimation : MonoBehaviour
{
    private static int Val1 = 0;
    private static int Val2 = 1;
    private static int Val3 = 2;

  //Gents the number of word or sentences from the gs_sentence_retrieval
    private static int mainLength = 0;
    private static int subLength = 0;
    private static int sentLength = 0;

  //moves the main list forward three
    public static void Next()
    {
      
    }

    //Gets the sentences for the menu
    public static void Grab()
    {

    }

    // checks if a main button and/or sub button is selected
    public static bool Select() {
        //main selection on menu
        bool clicked = false;
        //secondary selection on menu
        bool subclicked = false;





        return clicked;
    }

    //triggers the python text to speech
    public static void Speaking() {
    }

    //Moves the main menu
    public static void Prev(bool n){


    }
}
