using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Components")]
    public PlayerMovement movement;

    public CapsuleCollider capsule;

    public Health health;
    public Camera camera;

    [Header("Camera")]
    public float sensX = 2;
    public float sensY = 2;
    public bool clampVerticalRotation = true;
    public float minX = -90;
    public float maxX = 90;
    public bool smooth;
    public float smoothTime = 5f;

    [HideInInspector] public Quaternion characterTargetRot;
    [HideInInspector] public Quaternion cameraTargetRot;

    public Transform firstPersonParent;
    public Vector3 headPosition { get { return firstPersonParent.position + new Vector3(0, 0, 0.1f); } }
    private Vector3 originalCameraPosition;

    // the layer mask to use when trying to detect view blocking
    // (this way we dont zoom in all the way when standing in another entity)
    // (-> create a entity layer for them if needed)
    public LayerMask viewBlockingLayers;
    public float zoomSpeed = 0.5f;
    public float distance = 0;
    public float minDistance = 0;
    public float maxDistance = 7;

    [Header("Offsets - Standing")]
    public Vector2 firstPersonOffsetStanding = Vector2.zero;

    [Header("Offsets - Crouching")]
    public Vector2 firstPersonOffsetCrouching = Vector2.zero;

    public float crouchOriginMultiplier = 0.65f;

    public Vector3 lookPositionFar
    {
        get
        {
            // if is the local player then headPosition instead
            Vector3 position = camera.transform.position;
            return position + camera.transform.forward * 9999f; // this might be really wrong
        }
    }

    public Vector3 lookPositionRaycasted
    {
        get
        {
            RaycastHit hit;
            //if (isLocalPlayer) might need this
            return Utils.RaycastWithout(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, gameObject)
                   ? hit.point
                   : lookPositionFar;
            //else
            //return Vector3.zero;
        }
    }

    void Start()
    {

        // only for local player
        camera.transform.SetParent(transform, false);
        camera.transform.rotation = Quaternion.identity;
        camera.transform.position = headPosition;

        // for all players
        originalCameraPosition = camera.transform.localPosition;
        characterTargetRot = transform.localRotation;
        cameraTargetRot = camera.transform.localRotation;
    }

    void LookRotation(Transform character)
    {

        // only while cursor is locked, otherwise we are in a UI
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float horizontalRotation = Input.GetAxis("Mouse X") * sensX;
        float verticalRotation = Input.GetAxis("Mouse Y") * sensY;

        LookRotationChangeBy(character, horizontalRotation, verticalRotation);
    }

    public void LookRotationChangeBy(Transform character, float horizontalRotation, float verticalRotation)
    {
        characterTargetRot *= Quaternion.Euler(0, horizontalRotation, 0);
        cameraTargetRot *= Quaternion.Euler(-verticalRotation, 0, 0);

        if (clampVerticalRotation)
            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, characterTargetRot, smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = characterTargetRot;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, minX, maxX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    void Update()
    {
        

        // only while alive
        //if (health.current > 0) // for now
        //{
        // set to player parent already?
        if (camera.transform.parent != transform)
            InitializeForcedLook();
        LookRotation(transform);
        //}
    }

    void LateUpdate()
    {
        

        // we have to set the camera rotation before calculating the zoom
        // position, otherwise it won't be smooth and would overwrite each
        // other.
        camera.transform.localRotation = cameraTargetRot;

        // calculate target and zoomed position
        if (distance == 0) // first person
        {
            // we use the current head bone position as origin here
            // -> gets rid of the idle->run head change effect that was odd
            // -> gets rid of upper body culling issues when looking downwards
            Vector3 headLocal = transform.InverseTransformPoint(headPosition);
            Vector3 origin = Vector3.zero;
            Vector3 offset = Vector3.zero;

            if (movement.state == State.CROUCHING)
            {
                origin = headLocal * crouchOriginMultiplier;
                offset = firstPersonOffsetCrouching;
            }
            else
            {
                origin = headLocal;
                offset = firstPersonOffsetStanding;
            }

            // set final position
            Vector3 target = transform.TransformPoint(origin + offset);
            camera.transform.position = target;
        }

    }

    public bool InFirstPerson()
    {
        return distance == 0;
    }

    public void InitializeForcedLook()
    {
        camera.transform.SetParent(transform, false);
        characterTargetRot = transform.localRotation; // initial rotation := where we looked at before freelook
        //ik.lookAtBodyWeightActive = true;
    }

}