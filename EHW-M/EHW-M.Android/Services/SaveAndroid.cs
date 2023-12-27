using System;
using System.IO;
using Android.Content;
using Java.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.OS;
using Android.Provider;
using Android.Widget;
using EHW_M.Droid.Services;
using EHWM.Services;
using Java.Util;
using System.Collections.Generic;
using System.Linq;

[assembly: Dependency(typeof(SaveAndroid))]
namespace EHW_M.Droid.Services {
    class SaveAndroid : ISave {
        //Method to save document as a file in Android and view the saved document
        public async Task SaveAndView(string fileName, String contentType, MemoryStream stream) {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q) {
                var fileUri = saveToDownloads(stream);

                Intent intent = new Intent();
                intent.SetPackage("com.bixolon.sample");

                PackageManager pm = Android.App.Application.Context.PackageManager;
                List<ResolveInfo> resolveInfos = pm.QueryIntentActivities(intent, 0).ToList();
                Collections.Sort(resolveInfos, new ResolveInfo.DisplayNameComparator(pm));

                if (resolveInfos.Count > 0) {
                    ResolveInfo launchable = resolveInfos[0];
                    ActivityInfo activity = launchable.ActivityInfo;
                    ComponentName name = new ComponentName(activity.ApplicationInfo.PackageName,
                            activity.Name);
                    Intent i = new Intent(Intent.ActionMain);

                    i.SetFlags(ActivityFlags.NewTask |
                            ActivityFlags.ResetTaskIfNeeded);
                    i.SetComponent(name);

                    Android.App.Application.Context.StartActivity(i);
                }
            }
            else {
                string exception = string.Empty;
                string root = null;

                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted) {
                    ActivityCompat.RequestPermissions((Android.App.Activity)Forms.Context, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
                }

                if (Android.OS.Environment.IsExternalStorageEmulated) {
                    root = Android.OS.Environment.ExternalStorageDirectory.ToString();
                }
                else
                    root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

                Java.IO.File myDir = new Java.IO.File(root + "/Syncfusion");
                try {
                    myDir.Mkdir();
                }
                catch (Exception e) {

                }
                Java.IO.File file = new Java.IO.File(myDir, fileName);

                if (file.Exists()) file.Delete();

                try {
                    FileOutputStream outs = new FileOutputStream(file);
                    outs.Write(stream.ToArray());

                    outs.Flush();
                    outs.Close();
                }
                catch (Exception e) {
                    exception = e.ToString();
                }
                if (file.Exists() && contentType != "application/html") {
                    string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
                    string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);

                    Intent intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage("com.bixolon.sample");
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
        }

        public Android.Net.Uri saveToDownloads(MemoryStream root) {
            Android.Net.Uri contentCollection = null;


            ContentResolver resolver = Android.App.Application.Context.ContentResolver;


            contentCollection = MediaStore.Downloads.GetContentUri(MediaStore.VolumeExternalPrimary);

            ContentValues valuesmedia = new ContentValues();
            valuesmedia.Put(MediaStore.MediaColumns.DisplayName, "Output.pdf");
            valuesmedia.Put(MediaStore.MediaColumns.MimeType, "application/pdf");
            valuesmedia.Put(MediaStore.MediaColumns.RelativePath, Android.OS.Environment.DirectoryDownloads);

            Android.Net.Uri savedPdfUri = resolver.Insert(contentCollection, valuesmedia);
            try {
                Stream outdata = resolver.OpenOutputStream(savedPdfUri);
                outdata.Write(root.ToArray());

                valuesmedia.Clear();
                Toast.MakeText(Android.App.Application.Context, "Pdf saved to Downloads", ToastLength.Short).Show();
            }
            catch (Exception exception) {
                Toast.MakeText(Android.App.Application.Context, "Pdf not saved to Downloads", ToastLength.Short).Show();
            }
            return savedPdfUri;
        }

    }
}