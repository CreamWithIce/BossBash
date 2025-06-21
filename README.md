![image](images/TitleCard.png)
## Welcome!
<p>
Hi there!

This project was created  in the span of 3 months. I created this for Victorian Curriculum Education (VCE) Software Development units 3 & 4 assessment task.

Alongside being a VCE assessment, the other purpose was to expand my knowledge of Machine Learning (ML) by using the opportunity brought about by Unities MLAgent package to train an ML model to aim at a target given the targets position.

More details about the process can be found in the Development Diary

</p>


## Pre-requisites

<p>
I would highly recommend installing the following for the project to work as intended (based on what I used):

1. Unity Editor: [2023.2.12f1](https://unity.com/releases/editor/whats-new/2023.2.12#notes)

2. Python: [3.10.11](https://www.python.org/downloads/release/python-31011/) for ML

3. (If not installed) [Visual C++ Redistributable](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170)

NOTE: The project used CUDA for training the ML model. Please install the [NVIDIA drivers](https://www.nvidia.com/en-us/drivers/) if you have a compatible GPU
</p>


## Installation

- Clone the project and open in unity

- Open terminal in the project directory:
    1. run: `venv\scripts\activate`

    2. Afterwards run: `pip install -r Setup/requirements.txt` to install packages (~4 gb to install)
    
- Run inside of unity to play

- Enjoy :) 

(P.S. there is nintendo/xbox controller support and basic settings menu)

## Training
<p>
To train the underlying AI head to the training scene in unity

- Open up a terminal in project again:

    1.  Run: `venv\scripts\activate` to activate the virtual environment
    2. To train fill in: `mlagents-learn --run-id=(Insert ID here) --torch-device=cuda MLConfig/Training.yaml`

- Run unity project

To stop the training press exit run time to terminate training

</p>

## Images

![image](images/Game.png)
Playing the game

![image](images/Training.png)
AI Training

## Final notes
<p>
 
 - The way the code/comments have been formatted were based on the rubric criteria for the project for VCE Software Development

- I wanted to make the project aesthetically appealing and engaging so I added controller support, and gave it a lava theme based on feedback from peers. <br>
Controller provides an added difficulty as there isn't the "auto-aim" that is found with the mouse

- Unity physics is inconsistent with the character movement speed. Feel free to adjust it if it is too fast or slow under the player movement script

</p>

