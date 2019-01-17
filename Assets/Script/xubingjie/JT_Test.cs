/* 
describe ：
author ：wolin 
createtime ：#CreateTime#
version：v 1.0
*/
//using System.Collections;
using UnityEngine;

public class JT_Test : MonoBehaviour {

    #region Private Attribute

    #endregion

    #region Public Attribute
    public UISprite _sprite;
    #endregion

    #region Unity API
    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            _sprite.width = 760;
            _sprite.height = 202;
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            _sprite.width = 380;
            _sprite.height = 101;
        }
    }
    #endregion

    #region Public My_Method
    #endregion

    #region Private My_Method
    #endregion
}
