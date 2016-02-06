using UnityEngine;
using System.Collections;
using Motive.Core.Scripting;
using System;
using System.Linq;
using Motive.Core.Diagnostics;
using Motive.Unity.Media;
using Motive.Core.Timing;
using System.Collections.Generic;
using Motive.Unity.Timing;
using Motive.Core.Models;

static class PlayableContentRoute
{
    public const string Messages = "messages";
    public const string Screen = "screen";
    public const string Soundtrack = "soundtrack";
    public const string Ambient = "ambient";
}

class BatchContext
{
    public UnityTimer Timer { get; set; }
    public bool Abort { get; set; }
    public ResourceActivationContext ActivationContext { get; set; }
    public PlayableContent[] Playables { get; set; }
}

class WaitingPlayableContext
{
    public ResourceActivationContext ActivationContext { get; set; }
    public PlayableContent Playable { get; set; }
    public Action OnClose { get; set; }
}

public class ContentPlayer : SingletonComponent<ContentPlayer>
{

	Motive.Core.Diagnostics.Logger m_logger;
    Dictionary<string, BatchContext> m_containers;

    List<WaitingPlayableContext> m_waitingContexts;
    WaitingPlayableContext m_playingContext;

    protected override void Awake()
    {
		m_logger = new Motive.Core.Diagnostics.Logger(this);
        m_containers = new Dictionary<string, BatchContext>();
        m_waitingContexts = new List<WaitingPlayableContext>();

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        ScriptManager.Instance.ScriptsReset += ScriptManager_ScriptsReset;
    }

    void ScriptManager_ScriptsReset(object sender, EventArgs e)
    {
        m_containers.Clear();
        m_waitingContexts.Clear();
    }

    void PlayScreenContent(ResourceActivationContext context, PlayableContent playable, Action onClose)
    {
        var screenMsg = playable.Content as ScreenMessage;

        if (screenMsg != null && screenMsg.Responses != null && screenMsg.Responses.Length > 0)
        {
            var data = new ResourcePanelData<ScreenMessage>(context, screenMsg);

            PanelManager.Instance.Show<ScreenDialogPanel>(new ResourcePanelData<ScreenMessage>(context, screenMsg), onClose);
        }
        else if (playable.Content is MediaContent)
        {
            PanelManager.Instance.Show<ScreenImagePanel>(playable.Content, onClose);
        }
        else if (playable.Content is ITextMediaContent)
        {
            PanelManager.Instance.Show<TextMediaPopupPanel>(playable.Content, onClose);
        }
        else
        {
            m_logger.Error("Unsupported content type with route=screen: {0}",
                playable.Content == null?"null":playable.Content.Type);
        }
    }

    void PlayMessagesContent(ResourceActivationContext context, PlayableContent playable, Action onClose)
    {
        // Currently messages only recognizes CharacterMessage types
        var charMsg = playable.Content as CharacterMessage;

        if (charMsg != null)
        {
            var data = new ResourcePanelData<CharacterMessage>(context, charMsg);

            if (charMsg.Responses != null && charMsg.Responses.Length > 0)
            {
                PanelManager.Instance.Show<CharacterDialogPanel>(data, onClose);
            }
            else
            {
                PanelManager.Instance.Show<CharacterMessagePanel>(data, onClose);
            }
        }
        else
        {
            m_logger.Error("Unknown content type with route=messages: {0}",
                playable.Content.Type);

            onClose();
        }
    }

    void PlayNextSequentialContent()
    {
        WaitingPlayableContext ctxt = null;

        lock (m_waitingContexts)
        {
            if (m_playingContext != null || m_waitingContexts.Count == 0)
            {
                return;
            }
            else
            {
                ctxt = m_waitingContexts[0];
                m_waitingContexts.RemoveAt(0);
                m_playingContext = ctxt;
            }
        }

        Action next = () =>
        {
            if (ctxt.OnClose != null)
            {
                ctxt.OnClose();
            }

            lock (m_waitingContexts)
            {
                m_playingContext = null;
            }

            PlayNextSequentialContent();
        };

        if (ctxt != null)
        {
            var playable = ctxt.Playable;

            switch (playable.Route)
            {
                case PlayableContentRoute.Messages:
                    PlayMessagesContent(ctxt.ActivationContext, playable, next);
                    break;
                case PlayableContentRoute.Screen:
                    PlayScreenContent(ctxt.ActivationContext, playable, next);
                    break;
                default:
                    next();
                    break;
            }
        }
    }

    void RouteContent(ResourceActivationContext activationContext, PlayableContent playable, Action onClose = null)
    {
        // Use the Route parameter to direct the content to different parts
        // of your game.
        switch (playable.Route)
        {
            case PlayableContentRoute.Messages:
            case PlayableContentRoute.Screen:
                m_waitingContexts.Add(new WaitingPlayableContext { ActivationContext = activationContext, Playable = playable, OnClose = onClose });
                PlayNextSequentialContent();
                break;
            case PlayableContentRoute.Ambient:
            case PlayableContentRoute.Soundtrack:
                AudioContentPlayer.Instance.PlayAudioContent(playable.Content as LocalizedAudioContent, playable.Route, onClose);
                break;
            default:
                // Uknown route!
                m_logger.Error("Unknown route in playable content: {0}", playable.Route);

                if (onClose != null)
                {
                    onClose();
                }
                break;
        }
    }

    void Play(BatchContext context, PlayableContent playable, Action onClose)
    {
        if (!context.ActivationContext.CheckEvent(playable.Id, "close"))
        {
            // If there's a script timer, use the activation time as a base to
            // play this content later.
            if (playable.Timer != null)
            {
                context.Timer = UnityTimer.Call(playable.Timer.GetNextFireTime(context.ActivationContext.ActivationTime),
                    () =>
                    {
                        RouteContent(context.ActivationContext, playable, () =>
                        {
                            context.ActivationContext.FireEvent(playable.Id, "close");

                            if (onClose != null)
                            {
                                onClose();
                            }
                        });
                    });
            }
            else
            {
                RouteContent(context.ActivationContext, playable, () =>
                {
                    context.ActivationContext.FireEvent(playable.Id, "close");

                    if (onClose != null)
                    {
                        onClose();
                    }
                });
            }
        }
        else
        {
            if (onClose != null)
            {
                onClose();
            }
        }
    }

    public void Play(ResourceActivationContext ctxt, PlayableContent playable, Action onClose = null)
    {
        BatchContext playableContext = new BatchContext
        {
            ActivationContext = ctxt,
            Playables = new PlayableContent[] { playable }
        };

        m_containers[playable.Id] = playableContext;

        Play(playableContext, playable, onClose);
    }

    void PlayNextFromBatch(BatchContext ctxt, PlayableContentBatch playableBatch, int idx)
    {
        if (idx < playableBatch.Playables.Length)
        {
            var playable = playableBatch.Playables[idx];

            Play(ctxt, playable, () =>
            {
                if (!ctxt.Abort)
                {
                    PlayNextFromBatch(ctxt, playableBatch, idx + 1);
                }
            });
        }
        else
        {
            ctxt.ActivationContext.Close();
        }
    }

    public void Play(ResourceActivationContext ctxt, PlayableContentBatch playableBatch)
    {
        BatchContext playableContext = new BatchContext
        {
            ActivationContext = ctxt,
            Playables = playableBatch.Playables
        };

        m_containers[playableBatch.Id] = playableContext;

        if (playableBatch.Playables != null && playableBatch.Playables.Length > 0)
        {
            PlayNextFromBatch(playableContext, playableBatch, 0);
        }
        else
        {
            ctxt.Close();
        }
    }

    public void StopPlaying(string resourceId)
    {
        BatchContext context = null;

        if (m_containers.TryGetValue(resourceId, out context))
        {
            m_containers.Remove(resourceId);

            if (context.Timer != null)
            {
                context.Timer.Cancel();
            }

            context.Abort = true;

            if (context.Playables != null)
            {
                foreach (var playable in context.Playables)
                {
                    var audioContent = playable.Content as LocalizedAudioContent;

                    if (audioContent != null)
                    {
                        AudioContentPlayer.Instance.StopPlaying(audioContent);
                    }

                    lock (m_waitingContexts)
                    {
                        m_waitingContexts.RemoveAll((_ctxt) => context.Playables.Contains(_ctxt.Playable));
                    }
                }
            }
        }
    }
}
