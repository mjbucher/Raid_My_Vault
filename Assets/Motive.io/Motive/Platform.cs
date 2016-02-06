using UnityEngine;
using System.Collections;
using Motive.Core.Media;
using Motive.Unity.Media;

public class Platform : SingletonComponent<Platform> {

    public IAudioPlayerChannel CreateAudioPlayerChannel()
    {
        UnityAudioPlayerChannel channel = gameObject.AddComponent<UnityAudioPlayerChannel>();

        return channel;
    }
}
