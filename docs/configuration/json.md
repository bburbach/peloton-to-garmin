---
layout: default
title: JSON Config File
parent: Configuration
nav_order: 0
---

# Json Config File

This is the preferred way to provide configuration details to P2G.  By default, P2G looks for a file named `configuration.local.json` in the same directory where the program is run.

The config file is written in JSON and supports hot-reload for all fields except the following:

1. `App.PollingintervalSeconds`
1. `Observability` Section

The config file is organized into the below sections.

| Section      | Description       |
|:-------------|:------------------|
| [App Config](#app-config) | This section provides global settings for the P2G application. |
| [Format Config](#format-config) | This section provides settings related to conversions and what formats should be created/saved.  |
| [Peloton Config](#peloton-config) | This section provides settings related to fetching workouts from Peloton.      |
| [Garmin Config](#garmin-config) | This section provides settings related to uploading workouts to Garmin. |
| [Observability Config](#observability-config) | This section provides settings related to Metrics, Logs, and Traces for monitoring purposes. |

## App Config

This section provides global settings for the P2G application.

```json
 "App": {
    "OutputDirectory": "./output",
    "EnablePolling": true,
    "PollingIntervalSeconds": 86400,
    "PythonAndGUploadInstalled": true,
    "CloseWindowOnFinish": false
  }
```

| Field      | Required | Default | Description |
|:-----------|:---------|:--------|:------------|
| OutputDirectory | no | `$PWD/output` | Where downloaded and converted files should be saved to. |
| EnablePolling  | no | `true` | `true` if you wish P2G to run continuously and poll Peloton for new workouts. |
| PollingIntervalSeconds | no | 3600 | The polling interval in seconds determines how frequently P2G should check for new workouts. Be warned, that setting this to a frequency of hourly or less may get you flagged by Peloton as a bad actor and they may reset your password. |
| CloseWindowOnFinish | no | `false` | `true` if you wish the console window to close automatically when the program finishes. Not that if you have Polling enabled the program will never 'finish' as it remains active to poll regularly. |

## Format Config

This section provides settings related to conversions and what formats should be created/saved.  P2G supports converting Peloton workouts into a variety of different formats.  P2G also lets you choose whether or not you wish to save a local copy when the conversion is completed. This can be useful if you wish to backup your workouts or upload them manually to a different service other than Garmin.

```json
"Format": {
    "Fit": true,
    "Json": false,
    "Tcx": false,
    "SaveLocalCopy": false,
    "IncldudeTimeInHRZones": false,
    "IncludeTimeInPowerZones": false,
    "DeviceInfoPath": "./deviceInfo.xml",
    "Cycling": {
      "PreferredLapType": "Class_Targets"      
    },
    "Running": {
      "PreferredLapType": "Distance"
    }
  }
```

| Field      | Required | Default | Description |
|:-----------|:---------|:--------|:------------|
| Fit | no | `false` | `true` indicates you wish downloaded workouts to be converted to FIT |
| Json | no | `false` | `true` indicates you wish downloaded workouts to be converted to JSON  |
| Tcx  | no | `false` | `true` indicates you wish downloaded workouts to be converted to TCX |
| SaveLocalCopy | no | `false` | `true` will save any converted workouts to your specified [OutputDirectory](#app-config) |
| IncludeTimeInHRZones | no | `false` | **Only use this if you are unable to configure your Max HR on Garmin Connect.** When set to True, P2G will attempt to capture the time spent in each HR Zone per the data returned by Peloton. See [understanding custom zones](#understanding-custom-zones).
| IncludePowerInHRZones  | no | `false` | **Only use this if you are unable to configure your FTP and Power Zones on Garmin Connect.** When set to True, P2G will attempt to capture the time spent in each Power Zone per the data returned by Peloton. See [understanding custom zones](#understanding-custom-zones). |
| DeviceInfoPath | no | `null` | The path to your `deviceInfo.xml` file. See [providing device info](#custom-device-info) |
| Cycling | no | `null` | Configuration specific to Cycling workouts. |
| Cycling.PreferredLapType | no | `Default` | The preferred [lap type to use](#lap-types). |
| Running | no | `null` | Configuration specific to Running workouts. |
| Running.PreferredLapType | no | `Default` | The preferred [lap type to use](#lap-types). |

### Understanding Custom Zones

Garmin Connect expects that users have a registered device and they expect users have set up their HR and Power Zones on that device. However, this presents a problem if you either A) do not have a device capable of tracking Power or B) do not have a Garmin device at all.

The most common scenario for Peloton users is A, where they do not own a Power capable Garmin device and therefore are not able to configure their Power Zones in Garmin Connect.  If you do not have Power or HR zones configured in Garmin Connect then you are not able to view accurate `Time In Zones` charts for a given workout.

P2G provides a work around for this by optionally enriching the workout with the `Time In Zones` data with one caveat: the chart will not display the range value for the zone.

![Example Cycling Workout](https://github.com/philosowaffle/peloton-to-garmin/blob/master/images/missing_zone_values.png?raw=true "Example Missing Zone Values")

This is only available when generating and uploading the [FIT](#garmin-config) format.

### Custom Device Info

By default, P2G using a custom device when converting and upload workouts.  This device information is needed in order to count your Peloton workouts towards Challenges and Badges on Garmin. However, you may observe on Garmin Connect that your Peloton workouts will show a device image that does not match your personal device.

If you choose, you can provide P2G with your personal Device Info which will cause the Garmin workout to show the correct to device. Note, **this is completely optional and is only for cosmetic preference**, your workout will be converted, uploaded, and counted towards challenges regardless of whether this matches your personal device.

See [configuring device info]({{ site.baseurl }}{% link configuration/providing-device-info.md %}) for detailed steps on how to create your `deviceInfo.xml`.

### Lap Types

P2G supports several different strategies for creating Laps in Garmin Connect.  If a certain strategy is not available P2G will attempt to fallback to a different strategy.  You can override this behavior by specifying your preferred Lap type in the config. When `PreferredLapType` is set, P2G will first attempt to generate your preferred type and then fall back to the default behavior if it is unable to.  By default P2G will:

1. First try to create laps based on `Class_Targets`
1. Then try to create laps based on `Class_Segments`
1. Finally fallback to create laps based on `Distance`

| Strategy  | Config Value | Description |
|:----------|:-------------|:------------|
| Class Targets | `Class_Targets` | If the Peloton data includes Target Cadence information, then laps will be created to match any time the Target Cadence changed.  You must use this strategy if you want the Target Cadence to show up in Garmin on the Cadence chart. |
| Class Segments | `Class_Segments` | If the Peloton data includes Class Segment information, then laps will be created to match each segment: Warm Up, Cycling, Weights, Cool Down, etc. |
| Distance | `Distance` | P2G will caclulate Laps based on distance for each 1mi or 1km based on your distance setting in Peloton. |

## Peloton Config

This section provides settings related to fetching workouts from Peloton.

```json
"Peloton": {
    "Email": "peloton@gmail.com",
    "Password": "peloton",
    "NumWorkoutsToDownload": 1,
    "ExcludeWorkoutTypes": [ "meditation" ]
  }
```

⚠️ Your username and password for Peloton and Garmin Connect are stored in clear text, which **is not secure**. Please be aware of the risks. ⚠️

| Field      | Required | Default | Description |
|:-----------|:---------|:--------|:------------|
| Email | **yes** | `null` | Your Peloton email used to sign in |
| Password | **yes** | `null` | Your Peloton password used to sign in |
| NumWorkoutsToDownload | no | 5 | The default number of workouts to download. See [choosing number of workouts to download](#choosing-number-of-workouts-to-download).  Set this to `0` if you would like P2G to prompt you each time for a number to download. |
| ExcludeWorkoutTypes | no | none | A comma separated list of workout types that you do not want P2G to download/convert/upload. See [example use cases](#exclude-workout-types) below. |

### Choosing Number of Workouts To Download

When choosing the number of workouts P2G should download each polling cycle its important to keep your configured [PollingInterval](#app-config) in mind. If, for example, your polling interval is set to hourly, then you may want to set `NumWorkoutsToDownload` to 4 or greater. This ensures if you did four 15min workouts during that hour they would all be captured.

### Exclude Workout Types

Example use cases:

1. You take a wide variety of Peloton classes, including meditation and you want to skip uploading meditation classes.
1. You want to avoid double-counting activities you already track directly on a Garmin device, such as outdoor running workouts.

The available values are:

```json
		Cycling
		BikeBootcamp
		TreadmillRunning
		OutdoorRunning
		TreadmillWalking
		OutdoorWalking
		Cardio
		Circuit
		Strength
		Stretching
		Yoga
		Meditation
```

## Garmin Config

This section provides settings related to uploading workouts to Garmin.

```json
"Garmin": {
    "Email": "garmin@gmail.com",
    "Password": "garmin",
    "Upload": false,
    "FormatToUpload": "fit",
    "UploadStrategy": 2
  }
```

⚠️ Your username and password for Peloton and Garmin Connect are stored in clear text, which **is not secure**. Please be aware of the risks. ⚠️

| Field      | Required | Default | Description |
|:-----------|:---------|:--------|:------------|
| Email | **yes - if Upload=true** | `null` | Your Garmin email used to sign in |
| Password | **yes - if Upload=true** | `null` | Your Garmin password used to sign in |
| Upload | no | `false` |  `true` indicates you wish downloaded workouts to be automatically uploaded to Garmin for you. |
| FormatToUpload | no | `fit` | Valid values are `fit` or `tcx`. Ensure the format you specify here is also enabled in your [Format config](#format-config) |
| UploadStrategy | **yes if Upload=true** | `null` |  Allows configuring different upload strategies for syncing with Garmin. Valid values are `[0 - PythonAndGuploadInstalledLocally, 1 - WindowsExeBundledPython, 2 - NativeImplV1]`. See [upload strategies](#upload-strategies) for more info. |

### Upload Strategies

Because Garmin does not officially support 3rd party uploads by small projects like P2G, over time we have occassionally encountered upload issues.  This has caused P2G's upload strategy to evolve.  Based on your installation method and or geo location, different upload strategies have worked for different people at different times.

If you are just getting started with P2G, I recommend you start with upload strategy `2 - NativeImplV1`.  You can find more details about the strategies below.

| Strategy  | Config Value | Description |
|:----------|:-------------|:------------|
| PythonAndGuploadInstalledLocally | 0 | The very first strategy P2G used. This assumes you have Python 3 and the [garmin-uploader](https://github.com/La0/garmin-uploader) python library already installed on your computer.  This strategy uses the `garmin-uploader` python library for handling all uploads to Garmin. |
| WindowsExeBundledPython | 1 | If you are running the windows executable version of P2G and would like to use the [garmin-uploader](https://github.com/La0/garmin-uploader) python library for uploads then use this strategy. |
| NativeImplV1 | 2 | **The most current and recommended upload strategy.** P2G preforms the upload to Garmin itself without relying on 3rd party libraries. |

## Observability Config

P2G supports publishing OpenTelemetry Metrics, Logs, and Trace. This section provides settings related to those pillars.

The Observability config section contains three main sub-sections:

1. [Prometheus](#prometheus-config) - Metrics
1. [Jaeger](#jaeger-config) - Traces
1. [Serilog](#serilog-config) - Logs

```json
"Observability": {

    "Prometheus": {
      "Enabled": false,
      "Port": 4000
    },

    "Jaeger": {
      "Enabled": false,
      "AgentHost": "localhost",
      "AgentPort": 6831
    },

    "Serilog": {
      "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
      "MinimumLevel": "Information",
      "WriteTo": [
        { "Name": "Console" },
        {
          "Name": "File",
          "Args": {
            "path": "./output/log.txt",
            "rollingInterval": "Day",
            "retainedFileCountLimit": 7
          }
        }
      ]
    }
  }
```

### Prometheus Config

```json
"Prometheus": {
      "Enabled": false,
      "Port": 4000
    }
```

| Field      | Required | Default | Description |
|:-----------|:---------|:--------|:------------|
| Enabled | no | `false` | Whether or not to expose metrics. Metrics will be available at `http://localhost:{port}/metrics` |
| Port | no | `false` | The port the metrics endpoint should be served on. |

If you are using Docker, ensure you have exposed the port from your container.

#### Example Prometheus scraper config

```yaml
- job_name: 'p2g'
    scrape_interval: 60s
    static_configs:
      - targets: [<p2gIPaddress>:<p2gPort>]
    tls_config:
      insecure_skip_verify: true
```

### Jaeger Config

```json
"Jaeger": {
      "Enabled": false,
      "AgentHost": "localhost",
      "AgentPort": 6831
    }
```

| Field      | Required | Default | Description |
|:-----------|:---------|:--------|:------------|
| Enabled | no | `false` | Whether or not to generate traces. |
| AgentHost | **yes - if Enalbed=true** | `null` | The host address for your trace collector. |
| AgentPort | **yes - if Enabled=true** | `null` | The port for your trace collector. |

### Serilog Config

```json
"Serilog": {
      "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Sinks.Elasticsearch", "Serilog.Sinks.Grafana.Loki" ],
      "MinimumLevel": "Debug",
      "WriteTo": [
        { "Name": "Console" },
        {
          "Name": "File",
          "Args": {
            "path": "./output/log.txt",
            "rollingInterval": "Day",
            "retainedFileCountLimit": 7
          }
        },
        {
          "Name": "Elasticsearch",
          "Args": {
            "nodeUris": "http://192.168.1.95:9200",
            "indexFormat": "p2g-logs-{0:yyyy.MM.dd}"
          }
        },
        {
          "Name": "GrafanaLoki",
          "Args": {
            "uri": "http://192.168.1.95:3100",
            "textFormatter": "Serilog.Sinks.Grafana.Loki.LokiJsonTextFormatter, Serilog.Sinks.Grafana.Loki",
            "labels": [
              {
                "key": "app",
                "value": "p2g"
              }
            ]
          }
        }]
}
```

| Field      | Required | Default | Description |
|:-----------|:---------|:--------|:------------|
| Using | no | `null` | A list of sinks you would like use. The valid sinks are listed in the examplea above. |
| MinimumLevel | no | `null` | The minimum level to write. `[Verbose, Debug, Information, Warning, Error, Fatal]` |
| WriteTo | no | `null` | Additional config for various sinks you are writing to. |

More detailed information about configuring Logging can be found on the [Serilog Config Repo](https://github.com/serilog/serilog-settings-configuration#serilogsettingsconfiguration--).
