using Strateq.Core.Utilities;

namespace HB.Utilities
{
    public partial class SystemData : SystemDataCore
    {
        public struct CustomValidation
        {
            public const string Exist = "Exist";
            public const string NotFound = "Not Found";
            public const string Invalid = "Invalid";
            public const string Required = "Required";
            public const string InUsed = "InUsed";
        }

        public partial struct CPRegistrationStatus
        {
            public static string Accepted = "Accepted";
            public static string Pending = "Pending";
            public static string Rejected = "Rejected";
        }

        public partial struct CPOpertaionalStatus
        {
            public static string Open = "Open";
            public static string UnderMaintenance = "Under Maintenance";
            public static string Unavailable = "Unavailable";
            public static string Removed = "Removed";
        }

        public partial struct CPStatus
        {
            public const string Available = "Available";
            public const string Preparing = "Preparing";
            public const string Charging = "Charging";
            public const string SuspendedEVSE = "SuspendedEVSE";
            public const string SuspendedEV = "SuspendedEV";
            public const string Finishing = "Finishing";
            public const string Reserved = "Reserved";
            public const string Unavailable = "Unavailable";
            public const string Faulted = "Faulted";
            public const string OutOfService = "OutOfService";
        }

        public partial struct CPErrorCode
        {
            public const string ConnectorLockFailure = "ConnectorLockFailure";
            public const string EVCommunicationError = "EVCommunicationError";
            public const string GroundFailure = "GroundFailure";
            public const string HighTemperature = "HighTemperature";
            public const string InternalError = "InternalError";
            public const string LocalListConflict = "LocalListConflict";
            public const string NoError = "NoError";
            public const string OtherError = "OtherError";
            public const string OverCurrentFailure = "OverCurrentFailure";
            public const string OverVoltage = "OverVoltage";
            public const string PowerMeterFailure = "PowerMeterFailure";
            public const string PowerSwitchFailure = "PowerSwitchFailure";
            public const string ReaderFailure = "ReaderFailure";
            public const string ResetFailure = "ResetFailure";
            public const string UnderVoltage = "UnderVoltage";
            public const string WeakSignal = "WeakSignal";
            public const string VendorSpecific = "VendorSpecific";
        }

        public partial struct CPErrorCodeDescription
        {
            public const string ConnectorLockFailure = "Failure to lock or unlock connector";
            public const string EVCommunicationError = "Communication failure with the vehicle, might be Mode 3 or other communication protocol problem. This is not a real error in the sense that the Charge Point doesn’t need to go to the faulted state. Instead, it should go to the SuspendedEVSE state";
            public const string GroundFailure = "Ground fault circuit interrupter has been activated";
            public const string HighTemperature = "Temperature inside Charge Point is too high";
            public const string InternalError = "Error in internal hard- or software component";
            public const string LocalListConflict = "The authorization information received from the Central System is in conflict with the LocalAuthorizationList";
            public const string NoError = "No error to report";
            public const string OtherError = "Other type of error. More information in vendorErrorCode";
            public const string OverCurrentFailure = "Over current protection device has tripped";
            public const string OverVoltage = "Voltage has risen above an acceptable level";
            public const string PowerMeterFailure = "Failure to read electrical/energy/power meter";
            public const string PowerSwitchFailure = "Failure to control power switch";
            public const string ReaderFailure = "Failure with idTag reader";
            public const string ResetFailure = "Unable to perform a reset";
            public const string UnderVoltage = "Voltage has dropped below an acceptable level";
            public const string WeakSignal = "Wireless communication device reports a weak signal";
            public const string VendorSpecific = "Panel Open";
        }

        public partial struct CancelReservationStatus
        {
            public static string Accepted = "Accepted";
            public static string Rejected = "Rejected";
        }

        public partial struct CPTransaction
        {
            public static string Started = "Started";
            public static string Complete = "Complete";
            public static string Rejected = "Rejected";
           
        }

        public struct BreakdownErrorStatus
        {
            public static int Fixed = 1;
            public static int InProgress = 2;
        }

        public struct BreakdownErrorSeverity
        {
            public static int Unknown = 0;
            public static int Minor = 1;
            public static int Medium = 2;
            public static int High = 3;
            public static int Critical = 4;
        }

        public struct CPStatusColorCode
        {
            public static string InUse = "#05B052";
            public static string NotInUse = "#E82526";
            public static string Maintenance = "#FF7E03";
            public static string Unavailable = "#D9D9D9";
        }

        public struct CPChartDisplayStatus
        {
            public const string InUse = "In use";
            public const string NotInUse = "Not in use";
            public const string Maintenance = "Maintenance";
            public const string Unavailable = "Unavailable";
        }

        public struct RunningSequenceNumber
        {
            public const string TransactionId = "TransactionId";
            
        }

        public struct ProductType
        {
            public const string Ac = "AC";
            public const string Dc = "DC";
            public const string AcDc = "AC/DC";
        }
        
        public struct CPConnectorStatus
        {
            public const string InUse = "In Use";
            public const string Open = "Open";
            public const string OutOfService = "Out Of Service";
        }

        public struct MessageType
        {
            public const int CallTransaction = 1;
            public const int Call = 2;
            public const int CallResult = 3;
            public const int CallError = 4;
        }

        public struct CPAction
        {
            public const string StartTransaction = "StartTransaction";
            public const string StopTransaction = "StopTransaction";
            public const string BootNotification = "BootNotification";
            public const string Authorize = "Authorize";
            public const string DataTransfer = "DataTransfer";
            public const string StatusNotification = "StatusNotification";
            public const string MeterValues = "MeterValues";
            public const string Heartbeat = "Heartbeat";
            public const string FailTransaction = "FailTransaction";
            public const string Reset = "Reset";
            public const string RemoteStartTransaction = "RemoteStartTransaction";
            public const string RemoteStopTransaction = "RemoteStopTransaction";
            public const string ClearCache = "ClearCache";
            public const string UnlockConnector = "UnlockConnector";
            public const string GetLocalListVersion = "GetLocalListVersion";
            public const string SendLocalList = "SendLocalList";
            public const string UpdateFirmware = "UpdateFirmware";
            public const string FirmwareStatusNotification = "FirmwareStatusNotification";
            public const string TriggerMessage = "TriggerMessage";
            public const string ChangeAvailability = "ChangeAvailability";
        }

        public struct ResetType
        {
            public const string Hard = "Hard";
            public const string Soft = "Soft";
        }

        public struct Unit
        {
            public const string FOC = "FOC";
            public const string kWh = "kWh";
            public const string Minutes = "Minutes";
            public const string Hours = "Hours";
        }
        public struct FirmwareStatus
        {
            public const string Downloaded = "Downloaded";
            public const string DownloadFailed = "DownloadFailed";
            public const string Downloading = "Downloading";
            public const string Idle = "Idle";
            public const string InstallationFailed = "InstallationFailed";
            public const string Installing = "Installing";
            public const string Installed = "Installed";
        }
    }
}