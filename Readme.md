# Inverse kinematic

**This project is about creation of Inverse Kinematic using existing algorithm.**

<img src="./Screenshots/Dummy.png" width="200"/>

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

<img src="./Screenshots/Feature.png" width="400"/>

## Screenshots

### Leg IK on a step 

<div style="display:flex"> 
    <img src="./Screenshots/Step.png" width="200"/>
    <img src="./Screenshots/StepFront.png" width="200"/>
</div>

### Arm IK

<div style="display:flex"> 
    <img src="./Screenshots/Arm.png" width="200"/>
    <img src="./Screenshots/ArmFront.png" width="200"/>
    <img src="./Screenshots/FloatingCube.png" width="200"/>
</div>