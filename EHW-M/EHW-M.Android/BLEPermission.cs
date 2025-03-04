﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EHW_M.Droid {
    public class BLEPermission : Xamarin.Essentials.Permissions.BasePlatformPermission {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
{
    (Android.Manifest.Permission.BluetoothScan, true),
    (Android.Manifest.Permission.BluetoothConnect, true),
    (Android.Manifest.Permission.AccessCoarseLocation,true),
    (Android.Manifest.Permission.AccessFineLocation,true),
    (Android.Manifest.Permission.ReadMediaImages,true),
}.ToArray();
    }
}