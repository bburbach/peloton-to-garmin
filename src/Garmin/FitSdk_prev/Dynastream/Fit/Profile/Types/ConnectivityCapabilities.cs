#region Copyright
////////////////////////////////////////////////////////////////////////////////
// The following FIT Protocol software provided may be used with FIT protocol
// devices only and remains the copyrighted property of Garmin Canada Inc.
// The software is being provided on an "as-is" basis and as an accommodation,
// and therefore all warranties, representations, or guarantees of any kind
// (whether express, implied or statutory) including, without limitation,
// warranties of merchantability, non-infringement, or fitness for a particular
// purpose, are specifically disclaimed.
//
// Copyright 2020 Garmin Canada Inc.
////////////////////////////////////////////////////////////////////////////////
// ****WARNING****  This file is auto-generated!  Do NOT edit this file.
// Profile Version = 21.40Release
// Tag = production/akw/21.40.00-0-g813c158
////////////////////////////////////////////////////////////////////////////////

#endregion

namespace Dynastream.Fit
{
    /// <summary>
    /// Implements the profile ConnectivityCapabilities type as a class
    /// </summary>
    public static class ConnectivityCapabilities 
    {
        public const uint Bluetooth = 0x00000001;
        public const uint BluetoothLe = 0x00000002;
        public const uint Ant = 0x00000004;
        public const uint ActivityUpload = 0x00000008;
        public const uint CourseDownload = 0x00000010;
        public const uint WorkoutDownload = 0x00000020;
        public const uint LiveTrack = 0x00000040;
        public const uint WeatherConditions = 0x00000080;
        public const uint WeatherAlerts = 0x00000100;
        public const uint GpsEphemerisDownload = 0x00000200;
        public const uint ExplicitArchive = 0x00000400;
        public const uint SetupIncomplete = 0x00000800;
        public const uint ContinueSyncAfterSoftwareUpdate = 0x00001000;
        public const uint ConnectIqAppDownload = 0x00002000;
        public const uint GolfCourseDownload = 0x00004000;
        public const uint DeviceInitiatesSync = 0x00008000; // Indicates device is in control of initiating all syncs
        public const uint ConnectIqWatchAppDownload = 0x00010000;
        public const uint ConnectIqWidgetDownload = 0x00020000;
        public const uint ConnectIqWatchFaceDownload = 0x00040000;
        public const uint ConnectIqDataFieldDownload = 0x00080000;
        public const uint ConnectIqAppManagment = 0x00100000; // Device supports delete and reorder of apps via GCM
        public const uint SwingSensor = 0x00200000;
        public const uint SwingSensorRemote = 0x00400000;
        public const uint IncidentDetection = 0x00800000; // Device supports incident detection
        public const uint AudioPrompts = 0x01000000;
        public const uint WifiVerification = 0x02000000; // Device supports reporting wifi verification via GCM
        public const uint TrueUp = 0x04000000; // Device supports True Up
        public const uint FindMyWatch = 0x08000000; // Device supports Find My Watch
        public const uint RemoteManualSync = 0x10000000;
        public const uint LiveTrackAutoStart = 0x20000000; // Device supports LiveTrack auto start
        public const uint LiveTrackMessaging = 0x40000000; // Device supports LiveTrack Messaging
        public const uint InstantInput = 0x80000000; // Device supports instant input feature
        public const uint Invalid = (uint)0x00000000;


    }
}

