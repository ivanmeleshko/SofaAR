using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class ScreenShot : MonoBehaviour
{

    GameObject dialog = null;
    AndroidJavaObject currentActivity;

    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            dialog = new GameObject();
        }
#endif

        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }


    public void CRSaveScreenshot()
    {
        //StartCoroutine(TakeScreenshot());
        string myFileName = "AR_Screenshot" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".jpg";
        string myScreenshotLocation;

        ScreenCapture.CaptureScreenshot(myFileName);
        try
        {
            string path = Application.persistentDataPath;
            AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
            path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jc.GetStatic<string>("DIRECTORY_DCIM")).Call<string>("getAbsolutePath");
            NativeToolkit.SaveScreenshot(myFileName, path + "/VisEonARScreenshots");
            myScreenshotLocation = path + "/VisEonARScreenshots/" + myFileName;
            new WaitForSeconds(2);
            //REFRESHING THE ANDROID PHONE PHOTO GALLERY
            AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
            objActivity.Call("sendBroadcast", objIntent);

            SendToastyToast("Screenshot has been saved to " + myScreenshotLocation);
            //NativeToolkit.ShowConfirm("Screenshot", "Screenshot has been saved to " + path);
        }
        catch { }
    }


    public void SendToastyToast(string message)
    {
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", message);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_LONG"));//"LENGTH_SHORT"
        toast.Call("show");
    }


    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        //INITIAL SETUP
        string myFilename = "myScreenshot.png";
        string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
        //EXAMPLE OF DIRECTLY ACCESSING THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
        //EXAMPLE OF BACKING INTO THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = Application.persistentDataPath + "/../../../../DCIM/Camera/";
        //EXAMPLE OF DIRECTLY ACCESSING A CUSTOM FOLDER OF THE GALLERY
        string myFolderLocation = "/storage/emulated/0/DCIM/MyFolder/";
        string myScreenshotLocation = myFolderLocation + myFilename;

        //ENSURE THAT FOLDER LOCATION EXISTS
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }

        //TAKE THE SCREENSHOT AND AUTOMATICALLY SAVE IT TO THE DEFAULT LOCATION.
        ScreenCapture.CaptureScreenshot(myFilename); //Application.CaptureScreenshot() is deprecated
        yield return new WaitForSeconds(1f); // Wait for capturing complete into file

        //MOVE THE SCREENSHOT WHERE WE WANT IT TO BE STORED
        System.IO.File.Move(myDefaultLocation, myScreenshotLocation);

        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);
        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS COMPLETE

        //Application.OpenURL(myScreenshotLocation);
    }

}
