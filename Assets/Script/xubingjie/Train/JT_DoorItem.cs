using UnityEngine;
using DG.Tweening;
using System;
public class JT_DoorItem : MonoBehaviour {
    public Action actionComplete;
    private Transform door1;
    private Transform door2;

    private Vector3 endPos_door1 = new Vector3(-0.184f, 2.16923f, -1.91f);
    private Vector3 endPos_door2 = new Vector3(-0.146f, 2.170926f, -0.046f);

    private Vector3 originPos_door1;
    private Vector3 originPos_door2;

    private void Awake() {
        door1 = this.transform.Find("Chemen08");
        door2 = this.transform.Find("Chemen06");


        originPos_door1 = door1.localPosition;
        originPos_door2 = door2.localPosition;
    }

    public void SetOriginPos() {
        if (door1 == null || door2 == null) {
            return;
        }
        door1.localPosition = originPos_door1;
        door2.localPosition = originPos_door2;
    }


    public void ChangeDoorState(bool isOpen) {
        if (isOpen) {

            door1.DOLocalMove(endPos_door1, 2f);
            door2.DOLocalMove(endPos_door2, 2f).OnComplete(() => {
                if (actionComplete != null) {
                    actionComplete();
                }
            });
        }
        else {
            door1.DOLocalMove(originPos_door1, 2f);
            door2.DOLocalMove(originPos_door2, 2f).OnComplete(() => {
                if (actionComplete != null) {

                    actionComplete();
                }
            });
        }
    }
}
