using UnityEngine;
using System.Collections;
using isotope;

/// <summary>
/// Load assetbundle sample
/// </summary>
public class LoadAssetBundle : MonoBehaviour
{
	/// <summary>
	/// Base URL of assetbundle
	/// </summary>
	public string BaseURL;
	/// <summary>
	/// Name of assetbundle
	/// </summary>
	public string AssetBundleName;
	// Use this for initialization
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		string myurl = "http://www.towerfun.com/assert/xbjxbh.unity";
		int index = myurl.LastIndexOf("/");
		if (index != -1)
		{
			string url = myurl.Substring(0,index+1);
			string name = myurl.Substring(index+1,myurl.Length-index-1);
			Debug.Log(url);
			Debug.Log(name);
		}
	}
	
	IEnumerator Start()
	{
		AssetBundleContainer container = AssetBundleManager.Instance.LoadBundleAsync( this.BaseURL + AssetBundleManager.GetPlatformFolder( Application.platform ) + "/" + this.AssetBundleName );
		while( !container.IsReady )
			yield return 0;
		if( container.IsError )
		{
			Debug.LogError( container.ErrorMsg );
			yield break;
		}
		foreach( var asset in container.FileList )
		{
			Debug.Log( asset.Name + " in " + container.name );
		}

#if UNITY_5
		var flag = container.AssetBundle.LoadAsset<GameObject>( "Flag" );
#else
		var flag = container.AssetBundle.Load( "Flag", typeof(GameObject) ) as GameObject;
#endif
		Debug.Log( flag );
		if( flag )
		{
#if UNITY_5
			// Because you can't use ClothRenderer on Unity5.
			var go = Instantiate( flag, base.transform.position, base.transform.rotation ) as GameObject;
			Debug.Log( go );
			var basego = new GameObject( "Base" );
			basego.transform.position = - new Vector3( 0, 0, 5 );
			basego.transform.localScale *= 0.2f;
			go.transform.SetParent( basego.transform, false );
#else
			var go = Instantiate( flag,base.transform.position, base.transform.rotation );
			Debug.Log( go );
#endif
		}
		//AssetBundleManager.Instance.UnloadBundle( container );
	}

	// Update is called once per frame
	void Update()
	{
	}
}
