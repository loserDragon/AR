using UnityEngine;
using com.ootii.Messages;
using DG.Tweening;
public class JT_TrainControll : MonoBehaviour {

    public GameObject AR_Train;
    public GameObject train ;
    public Transform[] doorParent;
    public JT_Pantograph _JT_Pantograph; 
	private	 void	 Awake(){
		
		MessageDispatcher.AddListener("ON_TRACKING_FOUND", ON_TRACKING_FOUND);
		MessageDispatcher.AddListener("ON_TRACKING_LOST", ON_TRACKING_LOST);
	}
	
	void OnDestroy() {
		MessageDispatcher.RemoveListener("ON_TRACKING_FOUND", ON_TRACKING_FOUND);
		MessageDispatcher.RemoveListener("ON_TRACKING_LOST", ON_TRACKING_LOST);
	}
	
	private void ON_TRACKING_LOST(IMessage rMessage) {
		if (rMessage.Data.ToString() != "184")
			return;

        if (doorParent == null)
            return;
        
        for (int i = 0; i < doorParent.Length; i++)
        {
            JT_DoorItem[] _JT_DoorItem = doorParent[i].GetComponentsInChildren<JT_DoorItem>();
            if(_JT_DoorItem == null)
                continue;
            for (int j = 0; j < _JT_DoorItem.Length; j++)
            {
                if (_JT_DoorItem[j] == null)
                    continue;
                _JT_DoorItem[j].SetOriginPos();
                if (i == doorParent.Length -1 && j == _JT_DoorItem.Length - 1)
                {
                    _JT_DoorItem[j].actionComplete = null;
                }
            }
            _JT_DoorItem = null;
        }

        this.CancelInvoke("Handle_ActionComplete");
        //AR_Train.SetActive(false);
   
	}
	
	private void ON_TRACKING_FOUND(IMessage rMessage) {
		if (rMessage.Data.ToString() != "184")
			return;
        //AR_Train.SetActive(true);
        TrainEnter();
	}

    private void TrainEnter(){

        train.transform.localPosition = new Vector3(0f, 0f, -83.16f);
        train.transform.DOLocalMove(new Vector3(0f, 0f, 13.87f), 2f).OnComplete(() => {
            for (int i = 0; i < doorParent.Length; i++) {
                JT_DoorItem[] _JT_DoorItem = doorParent[i].GetComponentsInChildren<JT_DoorItem>();
                for (int j = 0; j < _JT_DoorItem.Length; j++) {
                    if (_JT_DoorItem[j] == null)
                        continue;
                    _JT_DoorItem[j].ChangeDoorState(true);
                    if (i == doorParent.Length - 1 && j == _JT_DoorItem.Length - 1) {
                        _JT_DoorItem[j].actionComplete = () => { this.Invoke("Handle_ActionComplete", 2f); Debug.LogError(" 关门 。。。"); };

                    }
                }
            }
        });
    }

    void Handle_ActionComplete()
    {
        for (int i = 0; i < doorParent.Length; i++)
        {
            JT_DoorItem[] _JT_DoorItem = doorParent[i].GetComponentsInChildren<JT_DoorItem>();
            for (int j = 0; j < _JT_DoorItem.Length; j++)
            {
                if (_JT_DoorItem[j] == null)
                    continue;
                _JT_DoorItem[j].ChangeDoorState(false);
            }
            _JT_DoorItem = null;
        }
    }
}
