Baseline functionality for two or more people to collaborate remotely in VR. Try out this Unity project to experiment with multiplayer virtual reality experiences using the Leap Motion controller and an Oculus Rift or HTC Vive.

##Features include:   

* Drawing  
  - Pinch thumb and index finger on same hand together and hold. Move hand in this pinching position to see line appear. 
  - Erase your lines by touch the cube floating around your waist.
* Voice chat  
  - Wear headphones.
* Avatar  
  - Hand tracking using Leap Motion.
* Multiplayer networking   
  - Handled by Photon Unity Networking. See below to set this up before running.
* Timer  
  - Length of day night cycle is used to represent time .
  - Activate by having all players make and hold a thumbs up gesture.
  - Change the length of the timer in the RoomManager. 
  - Change the start and end time of day in the SkyController.
* High Five

##Hardware requirements:
* Oculus or HTC Vive
* Leap Motion sensor (mounted on front of headset)
* Unity 5.4
* Minimum system requirements: 
  - Windows 10 PC
  - Intel Core i5- 4590 
  - 8 GB RAM
  - Geforce GTX 970 or greater
  - 3x USB 3.0
* Make sure your graphics card, oculus, htc vive and leap motion drivers and firmware are all up to date!


##Photon Network setup before running app:
Before running the project, you must add your own Photon IDs to the PhotonServerSettings.txt (Photon Unity Networking > Resources).

Follow these instructions to obtain your IDs: 
* Photon App ID [https://doc.photonengine.com/en/realtime/current/getting-started/obtain-your-app-id] (https://doc.photonengine.com/en/realtime/current/getting-started/obtain-your-app-id)
* Photon Voice ID
  - Logged into your Photon account, click in the top right corner on Dashboard > Voice.
  - Select "Create New Vocie App" and enter any information.
  
##Note:
We will be adding git issues shortly to track already known bugs.
