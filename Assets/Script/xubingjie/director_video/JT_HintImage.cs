/* 
describe ：
author ：wolin 
createtime ：#CreateTime#
version：v 1.0
*/
//using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class JT_HintImage : MonoBehaviour {

    #region Private Attribute
    public JT_SliderImage _JT_SliderImage;
    #endregion

    #region Public Attribute
    #endregion

    #region Unity API
    private void Awake() {
        this.GetComponent<Button>().onClick.AddListener(OnCLickBtn) ;
        if (!PlayerPrefs.HasKey(JT_PlayerPrefabsMsg.IsShowHint)) {
            _JT_SliderImage.enabled = false;
            this.Invoke("CloseHint", 5f);
            //PlayerPrefs.DeleteKey(JT_PlayerPrefabsMsg.IsShowHint);
        }
        else {
            this.gameObject.SetActive(false);
        }
    }

    private void OnCLickBtn() {
        this.Invoke("CloseHint", 1f);
    }

    #endregion

    #region Public My_Method
    #endregion

    #region Private My_Method

    private void CloseHint() {
        _JT_SliderImage.enabled = true;
        this.gameObject.SetActive(false);
        PlayerPrefs.SetInt(JT_PlayerPrefabsMsg.IsShowHint, 1);
    }
    #endregion
}
