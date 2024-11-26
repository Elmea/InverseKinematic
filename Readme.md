# Inverse kinematic

**This project is about creation of Inverse Kinematic using existing algorithm.**

<img src="screenshots/dummy.png" width="200"/>


The main scene allow to test IK in different condition. 

The character use IK on leg when it is not moving, and Ik on arm when it detect a wall in range.

## Key mapping

**ZQSD** : Character movement

**Mouse X/Y** : Camera movement

**Up/Down Arrow** : Modify camera height

## Supported algorithm and feature

### Algorithm

For now, **only CCD is supported**, later Fabrik and Jacobian could be added and will be selectionable in the algorithm enum.

### Bone constraint

Custom bone constraint is supported, it can be added by simply add the bone constraint component to a bone. In its setup, angle are in degrees and in a range of [-180 ; 180]

*Example of the inverse kinematic class*

<img src="screenshots/Feature.png" width="400"/>

## Screenshots

### Leg IK on a step 

<img src="screenshots/Step.png" width="200"/>
<img src="screenshots/StepFront.png" width="200"/>

### Arm IK

<img src="screenshots/Arm.png" width="200"/>
<img src="screenshots/ArmFront.png" width="200"/>
<img src="screenshots/FloatingCube.png" width="200"/>