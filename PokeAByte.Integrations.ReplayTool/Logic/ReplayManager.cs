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
/*
 * TODO:
 * AddSaveRange (*)
 * UpdateSave
 * DeleteSave
 *
 * (*) Need to think about if I want to include these here since they require the MainForm or if I want to
 * further decouple this service from the GUI layer
 */
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
        _workerThread = new Thread(() => 
            ProcessSaveStates(_cancellationTokenSource.Token))
        {
            IsBackground = true,
            Name = "ProcessSaveStateQueueThread"
        };
        _workerThread.Start();
        _isRecording = true;
    }
    public void StopRecording()
    {
        _cancellationTokenSource.Cancel();
        _workerThread.Join();
        _recordingManager.Complete();
        _isRecording = false;
        _playbackManager.SetPlaybackStates(_recordingManager.GetAsPlaybackStateArray());
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
    private void ProcessSaveStates(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            while (_saveStateQueue.TryDequeue(out var saveState))
            {
                _recordingManager.Add(saveState, _recordingSettings.KeyframeIntervalCount);
            }
            Thread.Sleep(20);
        }
    }
    
    //Todo: probably switch to memory mapped files to save instead of constantly deleting and resaving the file
    /*public void SaveToFile(string path)
    {
        try
        {
            var serializeMessage = SerializationHelper.SerializeJsonToFile(_saveStateModel, path);
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

    public void LoadFromFile(string path)
    {
        try
        {
            var deserialized = SerializationHelper.DeserializeJsonFromFile<SaveStates>(path);
            if (!string.IsNullOrEmpty(deserialized.Message))
            {
                Log.Error(nameof(ReplayManager), 
                    "Failed to load from file: {deserialized.Message}", 
                    deserialized.Message);
            }
            else if (deserialized.Data is null)
            {
                Log.Error(nameof(ReplayManager), 
                    "Failed to load from file: deserialized.Data is null");
            }
            else
            {
                _saveStateModel = deserialized.Data;
                BuildSaveStates();
            }
        }
        catch (Exception e)
        {
            Log.Error(nameof(ReplayManager), 
                "Failed to load from file: {e}", e);
        }
    }
    public int GetStateCount()
    {
        return _saveStateModel is null ? 0 : _saveStateModel.States.Count;
    }*/
    public void Reset()
    {
        _recordingManager.Reset();
        //clear the queue
        while (_saveStateQueue.TryDequeue(out _)) ;
    }
}