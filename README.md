# HoloAAC

The general prototype for this project is as follows:

Object Detection (Swati) --> Conversation Retrieval (Swati) --> UI Display (Adrienne) --> Object Detection. 

Ideally, this should be a continuous loop such that right after sentences are displayed on the UI, it should loop back to once again go through object detection. All the individual components such as conversation retrieval and UI display are finished. What is still needed to be done is integrate UPython, a tool used to connect Unity and Python scripts, within the front end and the back end. 

# Swati's Code:

The object detection and conversation retrieval scripts are done completely in Python. The code used for the object detection comes from the GrocerEye project, publicly available on Github. This code runs a pretrained yolov3 model on an expansive dataset consisting of grocery store items. The code used for the purposes of our prototype is contained in "inference_local.py." To view the results of the model on sample pictures, the inference_local.py script should be run in jupyter notebook. The Anaconda environment must be installed to do this.

Output of inference_local.py: You can provide this script with a path to an image, run the script, and a bounding box with the name of the object class in the upper left corner will be displayed as the output. You will ALSO be able to see the probability of detection for EACH object that was detected. The object with the highest probability or accuracy is the one displayed in the picture

Conversation retrieval is also done locally, however there are some issues with packages that prevent it from being run locally. This is discussed in more detail in the actual script called "gs_sentence_retrieval.py." This is the local version of the script, containing a different set of stopword removal packages. The google colab script is "updatedWeights_conv_retrieval.ipynb" and contains a more comprehensive stopword removal package, spacy. 

Output of updatedWeights_conv_retrieval.ipynb: This script takes in a local text file callled "grocery_store_sentences.txt" which contains a list of ~100 sentences relating to things commonly asked at the grocery store. The output is a table of weights that assigns a weight to each "distinguishing" word within the dataset. For more specifics on this, please refer to the script. 

For specific code inquiries, please review the comments in each script and/or reach out to rampa009@umn.edu

