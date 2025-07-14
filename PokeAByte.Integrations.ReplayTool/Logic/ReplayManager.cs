using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool.Logic;
//https://github.com/endel/FossilDelta/

public delegate void OnStateAdded(string stateName);
public sealed class ReplayManager
{
    private readonly RecordingSettings _recordingSettings;
    private readonly Recording _recordingManager;
    private readonly Playback _playbackManager;
    private readonly ConcurrentQueue<RecordedSaveState> _saveStateQueue = new();
    
    private Thread _workerThread;
    private CancellationTokenSource _cancellationTokenSource;
    
    private bool _isRecording = false;
    private bool _isPlayback = false;
    
    public OnStateAdded? StateAdded { get; set; }
    
    public ReplayManager(RecordingSettings recordingSettings)
    {
        _recordingManager = new Recording();
        _playbackManager = new Playback();
        _recordingSettings = recordingSettings;
    }

    public void StartRecording()
    {
        if (_isRecording || _isPlayback)
        {
            return;
        }
        _recordingManager.Reset();
        _cancellationTokenSource = new CancellationTokenSource();
        _workerThread = new Thread(ProcessSaveStates);
        _workerThread.IsBackground = true;
        _workerThread.Name = "ProcessSaveStateQueueThread";
        _workerThread.Start();
        _isRecording = true;
    }
    public void StopRecording()
    {
        _cancellationTokenSource.Cancel();
        _workerThread.Join();
        _recordingManager.Complete();
        _isRecording = false;
        var playbackStates = _recordingManager.GetAsPlaybackStateArray();
        Log.Error("", $"Playback states count: {playbackStates.Length}");;
        _playbackManager.SetPlaybackStates(playbackStates);
    }
    
    public void SaveState(int frame, 
        byte[] state, 
        long saveTimeMs,
        bool isKeyframe = false,
        bool isFlagged = false, 
        string flagName = "")
    {
        //Since we want to be on a different thread, let's make sure we make a local clone
        //of the state before we do anything else
        if (state.Clone() is not byte[] stateBytes)
        {
            throw new NullReferenceException("State is null");
        }
        //If the _saveStateModel is not created then this should be the 
        //very first state created
        if (!_recordingManager.HasInitialState)
        {
            _recordingManager.SetFirstState(stateBytes);
        }
        _saveStateQueue.Enqueue(new RecordedSaveState
        {
            Frame = frame,
            FlagName = flagName,
            IsFlagged = isFlagged,
            SaveTimeMs = saveTimeMs,
            StateDelta = [],
            FullState = stateBytes,
            IsKeyframe = isKeyframe
        });
    }
    private void ProcessSaveStates()
    {
        var token = _cancellationTokenSource.Token;
        while (!token.IsCancellationRequested)
        {
            while (_saveStateQueue.TryDequeue(out var saveState))
            {
                StateAdded?.Invoke(_recordingManager.Add(saveState, _recordingSettings.KeyframeIntervalCount));
            }
            Thread.Sleep(20);
        }
    }
    public void Reset()
    {
        _recordingManager.Reset();
        //clear the queue
        while (_saveStateQueue.TryDequeue(out _)) ;
    }

    public int GetFrameCount()
    {
        return _playbackManager.GetPlaybackStateCount();
    }

    public byte[] GetReconstructedState(int index)
    {
        Log.Error("", $"Getting state at index: {index}");
        return _playbackManager.GetReconstructedState(index);
    }
    
    public void SaveToFile(string path)
    {
        try
        {
            var serializeMessage = SerializationHelper.SerializeJsonToFile(_recordingManager.SaveAsReplayFile(), path);
            if (!string.IsNullOrEmpty(serializeMessage))
            {
                Log.Error(nameof(ReplayManager), 
                    "Failed to save to file: {serializeMessage}", 
                    serializeMessage);
            }
        }
        catch (Exception e)
        {
            Log.Error(nameof(ReplayManager), 
                "Failed to save to file: {e}",
                e);
        }
    }
}