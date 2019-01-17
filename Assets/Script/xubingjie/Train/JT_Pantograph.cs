using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JT_Pantograph : MonoBehaviour {

    public Animator _ainm;

    //Highlighter _high;
    //private void Start()
    //{
        //_high = this.transform.GetComponent<Highlighter>();
        //if(_high == null){
            //_high = this.gameObject.AddComponent<Highlighter>();
        //}
        //this.GetComponent<JT_AnimatorPlayFinish>().m_Finish_OpenDoor_2 = () => {
            //_high.ConstantOff();
        //};

        //this.GetComponent<JT_AnimatorPlayFinish>().m_Finish_CloseDoor_2 = () => {
            //_high.ConstantOff();
        //};
    //}
    /// <summary>
    /// Changes the state of the pantograph.
    /// </summary>
    /// <param name="isUp">If set to <c>true</c> is up.</param>
    public void ChangePantographState(bool isUp){
        //_high.ConstantOn(Color.blue);

        if(isUp){
            _ainm.SetBool("isUp",true);
        }
        else{
            _ainm.SetBool("isUp", false);
        }

    }
}
