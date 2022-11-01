using System.Collections.Generic;
using HoloRobot.Robot.Industrial;
using HoloRobot.Utils;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Protocols;
using UnityEngine;

public sealed class Ur10EIndustrialRobot : IndustrialRobot
{
    Ur10EIndustrialRobot()
    {
        OnConnect();
    }

    ~Ur10EIndustrialRobot()
    {
        OnDisconnect();
        Debug.Log("Created");

    }

    private void Awake()
    {
        Debug.Log("Created on awake");

        ConnectedEvent.AddListener(OnConnectedToROS);
    }

    private void LateUpdate()
    {
        if (RosConnector == null) return;
        if (!RosConnector.isConnected) return;
        if (JointStateSubscriber == null || !JointStateSubscriber.isSubscribed) return;

        SetJointPositionsToView(Joints, JointsOffset,
            Convertor.MultiplayVectros(JointsPivot, JointStateSubscriber.GetJointPositions()), Speed);
    }

    private void OnConnectedToROS(bool arg0)
    {
        if (arg0)
            JointStateSubscriber.Subscribe(RosConnector);
        else
            JointStateSubscriber.UnSubscribe();
    }

    private void SetJointPositionsToView(List<Transform> joints, Quaternion[] offset, Vector3[] vectors, float speed)
    {
        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].localRotation = Quaternion.RotateTowards(joints[i].localRotation,
                offset[i] * Quaternion.Euler(vectors[i]), Time.deltaTime * speed);
        }
    }

    public void OnConnect()
    {
        Connect("ws://172.31.1.33:9090", Protocol.WebSocketSharp, RosSocket.SerializerEnum.Newtonsoft_JSON);
    }

    public void OnDisconnect()
    {
        Disconnect();
    }

    public void OnAddCartesianGoal()
    {
        AddCartesianGoal();
    }

    public void OnAddJointGoal()
    {
        AddJointGoal();
    }

    public void OnClear()
    {
        ClearCartesianGoal();
        ClearJointGoal();
    }
}