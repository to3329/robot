
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using UnityEngine;

public class TurtleBot3Controller : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/cmd_vel";  // ROSトピック名
    public float linearSpeed = 0;          // 前進速度
    public float angularSpeed = 0;         // 回転速度

    [System.Obsolete]
    void Start()
    {
        // ROSConnectionを取得し、トピックにサブスクライブ
        ros = ROSConnection.instance;
        ros.Subscribe<TwistMsg>(topicName, ReceiveVelocity);
    }

    void Update()
    {
        // TurtleBot3の移動処理
        transform.Translate(Vector3.forward * linearSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * angularSpeed * Time.deltaTime);
    }

    // ROSからの速度メッセージを受信して処理
    void ReceiveVelocity(TwistMsg msg)
    {
        // ROSから受信した速度データをUnityのオブジェクトに反映
        linearSpeed = (float)msg.linear.x;
        angularSpeed = (float)msg.angular.z;

        Debug.Log("Received velocity: linear.x = " + linearSpeed + ", angular.z = " + angularSpeed);
    }
}
