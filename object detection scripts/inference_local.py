import numpy as np
import time
import cv2
import os
import matplotlib.pyplot as plt
#%matplotlib inline


# this inference script is mainly to run the pretrained model on a set of sample
# images to determine the object(s) being detected

# the author of this code is Rehaan Bhimani (bhimar@uw.edu) and this code is
# being used for the object detection purposes of our prototype 



# display function to show image on Jupyter
def display_img(img,cmap=None):
    fig = plt.figure(figsize = (12,12))
    plt.axis(False)
    ax = fig.add_subplot(111)
    ax.imshow(img,cmap)

# this is the path to the object names (or classes) - you will need to make sure
# to pull this file from the HoloAAC github repository

labelsPath = os.path.join(r"C:\Users\12243\Desktop\darknet\data\obj.names")
print(labelsPath)
LABELS = open(labelsPath).read().strip().split("\n")

# using the .backup file means we are using the most trained weights
# these two files are also found in the HoloAAC github repo and can be pulled
# the first file is the path to the pretrained weights
weightsPath = os.path.join(r"C:\Users\12243\Desktop\darknet\yolov3_custom_train_6000.weights")
configPath = os.path.join(r"C:\Users\12243\Desktop\darknet\yolov3_custom_test.cfg")


net = cv2.dnn.readNetFromDarknet(configPath,weightsPath)

# A util function to predict and display bounding boxes
def predict(image):
    
    # initialize a list of colors to represent each possible class label
    np.random.seed(42)
    COLORS = np.random.randint(0, 255, size=(len(LABELS), 3), dtype="uint8")
    (H, W) = image.shape[:2]
    
    # determine only the "ouput" layers name which we need from YOLO
    ln = net.getLayerNames()
    ln = [ln[i[0] - 1] for i in net.getUnconnectedOutLayers()]
    
    # construct a blob from the input image and then perform a forward pass of the YOLO object detector, 
    # giving us our bounding boxes and associated probabilities
    blob = cv2.dnn.blobFromImage(image, 1 / 255.0, (416, 416), swapRB=True, crop=False)
    net.setInput(blob)
    layerOutputs = net.forward(ln)
    
    boxes = []
    confidences = []
    classIDs = []
    threshold = 0.2
    
    # loop over each of the layer outputs
    for output in layerOutputs:
        # loop over each of the detections
        for detection in output:
            # extract the class ID and confidence (i.e., probability) of
            # the current object detection
            scores = detection[5:]
            classID = np.argmax(scores)
            confidence = scores[classID]

            # filter out weak predictions by ensuring the detected
            # probability is greater than the minimum probability
            # confidence type=float, default=0.5
            if confidence > threshold:
                # scale the bounding box coordinates back relative to the
                # size of the image, keeping in mind that YOLO actually
                # returns the center (x, y)-coordinates of the bounding
                # box followed by the boxes' width and height
                box = detection[0:4] * np.array([W, H, W, H])
                (centerX, centerY, width, height) = box.astype("int")

                # use the center (x, y)-coordinates to derive the top and
                # and left corner of the bounding box
                x = int(centerX - (width / 2))
                y = int(centerY - (height / 2))

                # update our list of bounding box coordinates, confidences,
                # and class IDs
                boxes.append([x, y, int(width), int(height)])
                confidences.append(float(confidence))
                classIDs.append(classID)

    # apply non-maxima suppression to suppress weak, overlapping bounding boxes
    idxs = cv2.dnn.NMSBoxes(boxes, confidences, threshold, 0.1)

    # ensure at least one detection exists
    if len(idxs) > 0:
        print("object(s) detected")
        # loop over the indexes we are keeping
        for i in idxs.flatten():
            # extract the bounding box coordinates
            (x, y) = (boxes[i][0], boxes[i][1])
            (w, h) = (boxes[i][2], boxes[i][3])

            # draw a bounding box rectangle and label on the image
            color = (255,0,0)
            cv2.rectangle(image, (x, y), (x + w, y + h), color, 2)
            text = "{}".format(LABELS[classIDs[i]], confidences[i])
            cv2.putText(image, text, (x +15, y + 20), cv2.FONT_HERSHEY_SIMPLEX,
                1, color, 2)
    return image


import random


# replace this path to a sample image on your local computer
# NOTE: make sure to run this code in jupyter to actually be able to see the bounding box

img_path = r"C:\Users\12243\OneDrive\Desktop\test_image.jpg"
img = cv2.imread(img_path)
img = cv2.cvtColor(img,cv2.COLOR_BGR2RGB)
print(img_path)
display_img(predict(img))
