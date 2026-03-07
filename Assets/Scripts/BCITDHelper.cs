using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BCIEssentials.Stimulus.Collections;
using BCIEssentials.Behaviours.Trials;
using BCIEssentials.Behaviours;

public class BCITDHelper : MonoBehaviour
{
    public static BCITDHelper instance;
    [SerializeField] StimulusPresenterCollection presenterCollection;
    [SerializeField] SSVEPTrialBehaviour sSVEPTrialBehaviour;
    [SerializeField] SpriteRenderer[] towerStims;
    public enum StimGroup {Towers, Menu, Upgrades}

    [SerializeField, Space]
    private BCIController bciController;

    //NOTE: Don't forget when adding stumuli buttons to stagger the 
    // framesOn and framesOff by one in editor. ie: [1,2], [2,3], [3,4] etc.

    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
        if(!bciController)
            bciController = GetComponent<BCIController>();

    }

    public void ActivateDefaultStimGroup()
    {
        ActivateStimGroup(StimGroup.Towers);
    }

    public void DisableAllStimGroups()
    {
        DisableStimGroup(StimGroup.Towers);
        DisableStimGroup(StimGroup.Upgrades);
    }

    public void ActivateStimGroup(StimGroup group)
    {
        if (group == StimGroup.Towers)
        {
            foreach (SpriteRenderer stim in towerStims)
            {
                stim.enabled = true;
                //Need to add the stimulus presenters to the SSVEP Trial Behaviour and the Stimulus Presenter COllection
                presenterCollection.Add(stim.GetComponent<CustomStimulusPresenter>());
                sSVEPTrialBehaviour.Presenters.Add(stim.GetComponent<CustomStimulusPresenter>());
            }
        }

        else if(group == StimGroup.Upgrades)
        {
            GameObject[] upgrades = GameObject.FindGameObjectsWithTag("Upgrade");
            foreach(GameObject stim in upgrades)
            {
                stim.GetComponent<SpriteRenderer>().enabled = true;
                //Need to add the stimulus presenters to the SSVEP Trial Behaviour and the Stimulus Presenter COllection
                presenterCollection.Add(stim.GetComponent<CustomStimulusPresenter>());
                sSVEPTrialBehaviour.Presenters.Add(stim.GetComponent<CustomStimulusPresenter>());
            }
        }

        //this is stupid but we have to toggle trial off and on again.
        bciController.InterruptTrial();
        bciController.StartTrial();
    }
    
    public void DisableStimGroup(StimGroup group)
    {
        if(group == StimGroup.Towers)
        {
            foreach(SpriteRenderer stim in towerStims)
            {
                stim.enabled = false;
            }
        }
        else if(group == StimGroup.Upgrades)
        {
            GameObject[] upgrades = GameObject.FindGameObjectsWithTag("Upgrade");
            foreach(GameObject stim in upgrades)
            {
                stim.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
        //Clear the collection looking for/providing stimuli.
        presenterCollection.Clear(); 
        sSVEPTrialBehaviour.Presenters.Clear();
    }

}
