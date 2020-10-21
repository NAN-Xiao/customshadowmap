using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class view : MonoBehaviour
{

    public Camera cam;
    public Matrix4x4 m;
    public float distance;
    public Light light;

    public float size;


    public Camera lightcam;

    public RenderTexture shadowmap;

    public Material pcf;
    public void Update()
    {
       
    }
    // Start is called before the first frame update
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), ""))
        {
            Debug.LogError(cam.projectionMatrix);
            Debug.LogError(cam.worldToCameraMatrix);
          

        }
        m = cam.previousViewProjectionMatrix;
    }

    private void OnDrawGizmos()
    {
        var point =transform.position+ cam.transform.forward * distance;
        //center
        var fov = cam.fieldOfView / 2;
        var far = transform.position + cam.transform.forward * cam.farClipPlane;
        var near = transform.position + cam.transform.forward * cam.nearClipPlane;
        float asct = Screen.width / Screen.height;
        //tan

       // Debug.LogError(asct);



        Gizmos.color = Color.red;
        Gizmos.DrawSphere(far, 0.5f);
        Gizmos.DrawLine(cam.transform.position, far);


        //
        




        Gizmos.color = Color.white;
        Gizmos.DrawSphere(point, 0.5f);
        Gizmos.DrawLine(cam.transform.position, point);



       


        if(lightcam==null)
        {
            CreatLight();
        }


        UpdateCamera(point);

        UpdateMartix();




    }


    public void CreatLight()
    {
        lightcam = new GameObject("light").AddComponent<Camera>();
        lightcam.orthographic = true;
        lightcam.orthographicSize = 15;
        lightcam.cullingMask = 1<<LayerMask.NameToLayer("cast");
       // lightcam.aspect = 1;
    }

    public void UpdateCamera(Vector3 pos)
    {
        lightcam.transform.position = pos;
        lightcam.transform.forward = light.transform.forward;
        lightcam.transform.position += lightcam.transform.forward * -100;
        lightcam.farClipPlane = 500;
        if (lightcam.targetTexture == null)
        {
            shadowmap = new RenderTexture(1024, 1024, 0);
            shadowmap.format = RenderTextureFormat.R8;
            lightcam.targetTexture = shadowmap;
            Shader.SetGlobalTexture("_shadowmap", lightcam.targetTexture);
        }
    }
    public void  UpdateMartix()
    {

        var vp = GL.GetGPUProjectionMatrix(lightcam.projectionMatrix,false)*lightcam.worldToCameraMatrix ;
        Shader.SetGlobalMatrix("vp", vp);
        
        if (pcf==null)
        {
            Graphics.Blit()
        }
      

    }

}
