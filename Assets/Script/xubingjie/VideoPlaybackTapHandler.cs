/*============================================================================== 
 * Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;
using Vuforia;
using com.ootii.Messages;

/// <summary>
/// Specialized tap handler class for video playback.
/// </summary>
public class VideoPlaybackTapHandler : TapHandler
{
    protected override void OnSingleTapConfirmed()
	{
	    base.OnSingleTapConfirmed();
	    MessageDispatcher.SendMessage(this, "ON_MESSAGE_OnSingleTapConfirmed", "", EnumMessageDelay.IMMEDIATE);
    }

    /// <summary>
    /// Overriding base class implementation for double tap handling
    /// </summary>
    protected override void OnDoubleTap()
    {
	    base.OnDoubleTap();
	    MessageDispatcher.SendMessage(this, "ON_MESSAGE_OnDoubleTap", "", EnumMessageDelay.IMMEDIATE);
    }


    protected override void OnSingleTap()
    {
        base.OnSingleTap();
    }
}
