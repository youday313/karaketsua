-----------------------------------------------------
            Arbor: State Diagram Editor
          Copyright (c) 2014 Cait Sith Ware
          http://caitsithware.com/wordpress/
          support@caitsithware.com
-----------------------------------------------------

Thank you for purchasing Arbor! 

[How to Update] 

1. Remove Arbor folder that is already 
2. Import Arbor

[Main flow]

1. Attaching the ArborFSM to GameObject. 
2. Click the Open Editor button in the Inspector of ArborFSM 
3. Create a State in Arbor Editor. 
4. Attaching the Behaviour in State. 
5. Connect the transition to the State from Behaviour.

[If you want to use Javascript or Boo] 

1. If you do not have a Plugins folder in Assets below, to create a Plugins folder 
2. Create Arbor folder in the Plugins folder 
3. Move into that folder Core folder in the Arbor / Scripts /

[Sample scene] 

Here are the sample scene. 
Assets/Arbor/Examples/Scenes/

[Document] 

Click here for detailed document. 
http://arbor.caitsithware.com/en/

(Please select from the sidebar if you want to English)

[Support]

Forum : http://forum-arbor.caitsithware.com/

Mail : support@caitsithware.com

[Update History] 

Ver 1.7.4:
- Add: Agent system Behaviour added.
- Add: uGUI system Behaviour added.
- Add: uGUI system Tween added.
- Add: SendEventGameObject added.
- Add: The pass-by-value function added to the SendMessageGameObject.
- Fix: AnimatorParameterReference of reference is to Fixed get an error when that did not refer to a AnimatorController.
- Other: Pull up on the Unity minimum operating version to 4.6.7f1 due to uGUI correspondence.

Ver 1.7.3:
- Add: OnMouse system Transition add
- Fix: move when the scroll position correction to the selected state
- Other: modified to sort the state list by name.
- Other: Arbor change to be able to place the infinite state also to the upper left of the Editor.
- Other: to renew the manual site.

Ver 1.7.2:
- Add: Add a comment node in ArborEditor.
- Add: corresponding to be able to search at the time of behavior added.
- Add: CalcAnimatorParameter added.
- Add: AnimatorStateTransition added.
- Add: add to be able to move to a transition source and a transition destination in the right-click on the transition line.
- Fix: Prefab source to the Fixed not correctly added to the Prefab destination and behavior added.
- Other: Renamed the ForceTransition to GoToTransition.
- Other: change so as not to omit the name of the built-in Behaviour that is displayed in the behavior added.
- Other: change so as not to display the built-in Behaviour to Add Component.

Ver 1.7.1:
- Add: Add the state list.
- Add: Add PropertyDrawer of ParamneterReference.
- Add: Add GUI, the ListGUI for the list that can be deleted elements.
- Fix: Fixed boolValue of had become int of CalcParameter.

Ver 1.7.0;
- Add: parameter container.
- Fix: OnStateBegin () If you have state transitions, fix than it so as not to run the Behaviour under.

Ver 1.6.3f1:
- Fixed an error that exits at Unity5 RC3.
- Unity5 RC3 by the corresponding Renamed OnStateEnter / OnStateExit to OnStateBegin / OnStateEnd.

Ver 1.6.3:
- Add the force flag in Transition. you can do to transition on the spot at the time of the call to be to true.
- Embedded documentation comments to the source code.
- Place the script reference to Assets / Arbor / Docs.
  Please open the index.html Unzip.

Ver 1.6.2:
- FIX: The Fixed a state can not transition in OnStateEnter.

Ver 1.6.1:
- FIX: Error is displayed if you press Grid button in the Mac environment.

Ver 1.6: 
- ADD: Resident state.
- ADD: Multilingual.
- ADD: Correspondence to be named to ArborFSM.
- FIX: Are not reflected in the snap interval when you change the grid size.
- FIX: Deal of the problems StateBehaviour is lost when you copy and paste a component of ArborFSM.
- FIX: Modified to send only to the state currently in effect the SendTrigger.
- FIX: StateBehaviour continues to move If you disable ArborFSM.

Ver 1.5: 
- ADD: Support for multiple selection of the state. 
- ADD: Support for shortcut key. 
- ADD: grid display support. 
- FIX: it placed in a state in which it is spread by default when adding Behaviour. 
- FIX: I react mouse over to the state is shifted while dragging StateLink.

ver 1.4: 
- ADD: Tween-based Behaviour added. 
  - Tween / Color 
  - Tween / Position 
  - Tween / Rotation 
  - Tween / Scale 
- ADD: The HideBehaviour to add attributes that do not appear in the Add Behaviour. 
- ADD: online help of the built-in display Behaviour from the Help button on the Behaviour.

ver 1.3: 
- ADD: Add built-in Behaviour. 
  - Audio / PlaySoundAtPoint 
  - GameObject / SendMessage 
  - Scene / LoadLevel 
  - Transition / Force 
- ADD: Copy and paste across the scene. 
- FIX: memory leak warning is displayed when you save the scene after which you copied the State. 
- FIX: arrow will remain when you scroll the screen to drag the connection of the StateLink.

ver 1.2: 
- ADD: Enabled check box of StateBehaviour. 
- FIX: Errors Occur When you release the maximization of Arbor Editor. 
- FIX: Warning of the new line of code can when you edit a C# script that generated.

ver 1.1: 
- ADD: script generation of Boo and JavaScript. 
- ADD: Copy and paste the State. 
- ADD: Copy and paste the StateBehaviour. 
- FIX: support when the script becomes Missing. 
- FIX: Fixed array of StateLink is not displayed.

ver 1.0.1: 
- FIX: Error in Unity4.5. 
- FIX: Arbor Editor is not repaint when running in the editor. 
- FIX: class name of the Inspector extension of ArborFSM.
