using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // WASD：前後左右の移動
    // QE：上昇・降下
    // 右ドラッグ：カメラの回転
    // 左ドラッグ：前後左右の移動
    // スペース：カメラ操作の有効・無効の切り替え
    // P：回転を実行時の状態に初期化する

    //マウス感度
    [SerializeField, Range(30.0f, 150.0f)] private float _mouseSensitive = 90.0f;

    //カメラ操作の有効無効
    private bool _cameraMoveActive = true;

    //カメラのtransform  
    private Transform _camTransform;

    //マウスの始点 
    private Vector3 _startMousePos;

    //カメラ回転の始点情報
    private Vector3 _presentCamRotation;

    private Vector3 _presentCamPos;

    //初期状態 Rotation
    private Quaternion _initialCamRotation;

    //UIメッセージの表示
    private bool _uiMessageActiv;

    void Start()
    {
        _camTransform = gameObject.transform;

        //初期回転の保存
        _initialCamRotation = gameObject.transform.rotation;
    }

    void Update()
    {

        CamControlIsActive(); //カメラ操作の有効無効

        if (_cameraMoveActive)
        {
            ResetCameraRotation(); //回転角度のみリセット
            CameraRotationMouseControl(); //カメラの回転 マウス
        }
    }

    //カメラ操作の有効無効
    public void CamControlIsActive()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _cameraMoveActive = !_cameraMoveActive;
        }
    }

    //回転を初期状態にする
    private void ResetCameraRotation()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.gameObject.transform.rotation = _initialCamRotation;
            Debug.Log("Cam Rotate : " + _initialCamRotation.ToString());
        }
    }

    //カメラの回転 マウス
    private void CameraRotationMouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePos = Input.mousePosition;
            _presentCamRotation.x = _camTransform.transform.eulerAngles.x;
            _presentCamRotation.y = _camTransform.transform.eulerAngles.y;
        }

        if (Input.GetMouseButton(0))
        {
            //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            float x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
            float y = (_startMousePos.y - Input.mousePosition.y) / Screen.height;

            //回転開始角度 ＋ マウスの変化量 * マウス感度
            float eulerX = _presentCamRotation.x - y * _mouseSensitive;
            float eulerY = _presentCamRotation.y + x * _mouseSensitive;

            _camTransform.rotation = Quaternion.Euler(eulerX, eulerY, 0);
        }
    }
}
