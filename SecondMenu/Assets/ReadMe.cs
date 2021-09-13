/* This file contains information on the project that has been created as well as the resources used during the project's creation
 * 
 * Below are the resources that are used to create part of the UI. You may notice that "Unity Compatibility" is mentioned under each resouces. 
 * This is due to the fact that each of these codes are generally created in different years therefore have a tendency to work on each Unity differently.
 * At this point the physical menu has been created and has been moved to Unity 2021 with out much error, but to place the whole MRTK onto 2021 will cause
 * errors.
 * 
 * Resources:
 *  Title: HololensCameraStream
 *  Link: https://github.com/VulcanTechnologies/HoloLensCameraStream
 *  Purpose: Find a way to collect the frames of the Hololens Camera
 *  Unity Compatibility: 2017
 * 
 *  Title: UPython
 *  Link: https://perfect-sauce-33c.notion.site/UPython2-7431b13d3f0f4a41aa6fb6e16da782a3
 *  Purpose: Connect the Python coded back end with the C# front end. Without the backend the menu only 
 *          clickes when a button is selected it cannot traverse it cannot use tts.
 *  Unity Compatibility: 2021
 * 
 *  Title: Mixed Reality ToolKit
 *  Link:https://github.com/Microsoft/MixedRealityToolkit-Unity
 *  Purpose: Runs the general
 *  Unity Compatibility: 2018
 *  
 *  Title: Cognitive Services TTS
 *  Link: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-text-to-speech?tabs=script%2Cwindowsinstall&pivots=programming-language-csharp
 *  Purpose: C# text to speech if you're so inclined. It also the TTS in other coding languages including python
 *  Unity Compatibility: At least 2018+ but could work on older 
 *  
 *  Title: Hololens Emulator
 *  Link:https://docs.microsoft.com/en-us/windows/mixed-reality/develop/platform-capabilities-and-apis/using-the-hololens-emulator
 *  Purpose: Helps with emulating the the menu code. You could also use the Unity play tester as well. On Unity at least 
 *          't' brings up the left hand and 'y' brings up the right hand. Hold 'control' and move the mouse when the hand is up to turn the hand.
 *  Unity Compatibility: N/a
 *  
 *  Title: Contact For questions
 *  Link: adriennehembrick@gmail.com
 *  Purpose: Email if there are any question and I will try to answer then to the best of my abilities
 *  
 * 
 * Files:
 *  Title: MenuMoveimation
 *  Purpose: This is the rough beginnings of a code that basically lets the menu traverse through different options. The far left sid of the menu
 *          are main options. This list of options will generally stay the same. The middle column are subcatagories it changes depending on which which item that is selected
 *          (e.g. if you select "apple" on the right column may change to have "bag", while when you select "milk" it may have "bag" and "2%".) The thrid colum are the sentences
 *          This depends on what you select from the the first two columns, and is also where the tts should in theory speak. This code could just be considered trash just through it out
 *          if it looks that bad.
 *  Methods:
 *          Next(): Adds one to the values of every Val number. In the code buttons are associate to number that Grab() will then use to grab numbers fromt he python code
 *          
 *          Grab(): grab sentences from python code and show onto the menu
 *          
 *          Select(): Checks if a main option button or a suboption button has been selected
 *          
 *          Speaking(): Collects the selected sentence and runs it through the tts Python code
 *          
 *          Prev(): Same idea as Next() but traverses menu buttons in reverse(moves you backwards in the options)
 *               
 * Issues:
 *  1. Fully connect the python backend to the Unity front end: 
 *      The Upython is added to Unity physically, but not in the code. Once the python code can be used it can be connected in the MenuMoveimation file. 
 *      If it is easier to just scrap the MenuMoveimation file, then just scrap it. It doesn't do anything at the movment without any python implementation
 *  2. Grabbing images:
 *      The HoloensCameraStream exmaple only really runs on python 2017. The DLLs are already created on Unity. There should be steps on how to run the HololensCameraStream on the Github site, but it is
 *      probably to some degree, outdated. Upython also does not support imagine transfer from C# to python and vice versa so there that as well.
 *
 * Other Notes:
 *  1. Technically there is a why to run Cognitive Services from Microsoft directly in Unity with a C# code, but it involves task thread and probably some Microsoft Azure, and I couldn't really figure out how
 *  to get that to function properly. So while I don't suggest this method, if you can get it to work, then get it to work 
 *  2. Good Luck
 */
