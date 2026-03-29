// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace ScrubJay.Enhancements.Exceptions;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// This covers 11 bits
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/0642cb2f-2075-4469-918c-4441e69c548a"/>
/// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winerror/nf-winerror-hresult_facility"/>
public enum Facility : ushort
{
    /// <summary>
    /// The default facility code.
    /// </summary>
    NULL = 0b_000_00000000,
    /// <summary>
    /// The source of the error code is an RPC subsystem.
    /// </summary>
    RPC = 0b_000_00000001,
    /// <summary>
    /// The source of the error code is a COM Dispatch.
    /// </summary>
    DISPATCH = 0b_000_00000010,
    /// <summary>
    /// The source of the error code is OLE Storage.
    /// </summary>
    STORAGE = 0b_000_00000011,
    /// <summary>
    /// The source of the error code is COM/OLE Interface management.
    /// </summary>
    ITF = 0b_000_00000100,
    /// <summary>
    /// This region is reserved to map undecorated error codes into HRESULTs.
    /// </summary>
    WIN32 = 0b_000_00000111,
    /// <summary>
    /// The source of the error code is the Windows subsystem.
    /// </summary>
    WINDOWS = 0b_000_00001000,
    /// <summary>
    /// The source of the error code is the Security API layer.
    /// </summary>
    SECURITY = 0b_000_00001001,
    /// <summary>
    /// The source of the error code is the Security API layer.
    /// </summary>
    SSPI = 0b_000_00001001,
    /// <summary>
    /// The source of the error code is the control mechanism.
    /// </summary>
    CONTROL = 0b_000_00001010,
    /// <summary>
    /// The source of the error code is a certificate client or server?
    /// </summary>
    CERT = 0b_000_00001011,
    /// <summary>
    /// The source of the error code is Wininet related.
    /// </summary>
    INTERNET = 0b_000_00001100,
    /// <summary>
    /// The source of the error code is the Windows Media Server.
    /// </summary>
    MEDIASERVER = 0b_000_00001101,
    /// <summary>
    /// The source of the error code is the Microsoft Message Queue.
    /// </summary>
    MSMQ = 0b_000_00001110,
    /// <summary>
    /// The source of the error code is the Setup API.
    /// </summary>
    SETUPAPI = 0b_000_00001111,
    /// <summary>
    /// The source of the error code is the Smart-card subsystem.
    /// </summary>
    SCARD = 0b_000_00010000,
    /// <summary>
    /// The source of the error code is COM+.
    /// </summary>
    COMPLUS = 0b_000_00010001,
    /// <summary>
    /// The source of the error code is the Microsoft agent.
    /// </summary>
    AAF = 0b_000_00010010,
    /// <summary>
    /// The source of the error code is.NET CLR.
    /// </summary>
    URT = 0b_10011,
    /// <summary>
    /// The source of the error code is the audit collection service.
    /// </summary>
    ACS = 0b_10100,
    /// <summary>
    /// The source of the error code is Direct Play.
    /// </summary>
    DPLAY = 0b_10101,
    /// <summary>
    /// The source of the error code is the ubiquitous memoryintrospection service.
    /// </summary>
    UMI = 0b_10110,
    /// <summary>
    /// The source of the error code is Side-by-side servicing.
    /// </summary>
    SXS = 0b_10111,
    /// <summary>
    /// The error code is specific to Windows CE.
    /// </summary>
    WINDOWS_CE = 0b_11000,
    /// <summary>
    /// The source of the error code is HTTP support.
    /// </summary>
    HTTP = 0b_11001,
    /// <summary>
    /// The source of the error code is common Logging support.
    /// </summary>
    USERMODE_COMMONLOG = 0b_11010,
    
    WER = 0b_11011,
    
    /// <summary>
    /// The source of the error code is the user mode filter manager.
    /// </summary>
    USERMODE_FILTER_MANAGER = 0b_11111,
    /// <summary>
    /// The source of the error code is background copy control
    /// </summary>
    BACKGROUNDCOPY = 0b_100000,
    
    /// <summary>
    /// The source of the error code is configuration services.
    /// </summary>
    CONFIGURATION = 0b_100001,
    WIA = 0b_100001,
    
    /// <summary>
    /// The source of the error code is state management services.
    /// </summary>
    STATE_MANAGEMENT = 0b_100010,
    /// <summary>
    /// The source of the error code is the Microsoft Identity Server.
    /// </summary>
    METADIRECTORY = 0b_100011,
    /// <summary>
    /// The source of the error code is a Windows update.
    /// </summary>
    WINDOWSUPDATE = 0b_100100,
    /// <summary>
    /// The source of the error code is Active Directory.
    /// </summary>
    DIRECTORYSERVICE = 0b_100101,
    /// <summary>
    /// The source of the error code is the graphics drivers.
    /// </summary>
    GRAPHICS = 0b_100110,
    
    /// <summary>
    /// The source of the error code is the user Shell.
    /// </summary>
    SHELL = 0b_100111,
    NAP = 0b_100111,
    
    /// <summary>
    /// The source of the error code is the Trusted Platform Module services.
    /// </summary>
    TPM_SERVICES = 0b_101000,
    /// <summary>
    /// The source of the error code is the Trusted Platform Module applications.
    /// </summary>
    TPM_SOFTWARE = 0b_101001,
    
    UI = 0b_101010,
    XAML = 0b_101011,
    ACTION_QUEUE = 0b_101100,
    
    /// <summary>
    /// The source of the error code is Performance Logs and Alerts
    /// </summary>
    PLA = 0b_110000,
    WINDOWS_SETUP = 0b_110000,
    
    /// <summary>
    /// The source of the error code is Full volume encryption.
    /// </summary>
    FVE = 0b_110001,
    /// <summary>
    /// he source of the error code is the Firewall Platform.
    /// </summary>
    FWP = 0b_110010,
    /// <summary>
    /// The source of the error code is the Windows Resource Manager.
    /// </summary>
    WINRM = 0b_110011,
    /// <summary>
    /// The source of the error code is the Network Driver Interface.
    /// </summary>
    NDIS = 0b_110100,
    /// <summary>
    /// The source of the error code is the Usermode Hypervisor components.
    /// </summary>
    USERMODE_HYPERVISOR = 0b_110101,
    /// <summary>
    /// The source of the error code is the Configuration Management Infrastructure.
    /// </summary>
    CMI = 0b_110110,
    /// <summary>
    /// The source of the error code is the user mode virtualization subsystem.
    /// </summary>
    USERMODE_VIRTUALIZATION = 0b_110111,
    /// <summary>
    /// The source of the error code is the user mode volume manager
    /// </summary>
    USERMODE_VOLMGR = 0b_111000,
    /// <summary>
    /// The source of the error code is the Boot Configuration Database.
    /// </summary>
    BCD = 0b_111001,
    /// <summary>
    /// The source of the error code is user mode virtual hard disk support.
    /// </summary>
    USERMODE_VHD = 0b_111010,
    /// <summary>
    /// The source of the error code is System Diagnostics.
    /// </summary>
    SDIAG = 0b_111100,
    
    /// <summary>
    /// The source of the error code is the Web Services.
    /// </summary>
    WEBSERVICES = 0b_111101,
    WINPE = 0b_111101,
    
    WPN = 0b_111110,
    WINDOWS_STORE = 0b_111111,
    INPUT = 0b_1000000,
    EAP = 0b_1000010,
    
    /// <summary>
    /// The source of the error code is a Windows Defender component.
    /// </summary>
    WINDOWS_DEFENDER = 0b_1010000,
    
    /// <summary>
    /// The source of the error code is the open connectivity service.
    /// </summary>
    OPC = 0b_1010001,
    
    XPS = 0b_1010010, // 0x52
    RAS = 0b_1010011, // 0x53
    MBN = 0b_1010100, // 0x54
    POWERSHELL = 0b_1010100, // 0x54
    EAS = 0b_1010101, // 0x55
    P2P_INT = 0b_1100010, // 0x62
    P2P = 0b_1100011, // 0x63
    DAF = 0b_1100100, // 0x64
    BLUETOOTH_ATT = 0b_1100101, // 0x65
    AUDIO = 0b_1100110, // 0x66
    VISUALCPP = 0b_1101101, // 0x6D
    SCRIPT = 0b_1110000, // 0x70
    PARSE = 0b_1110001, // 0x71
    BLB = 0b_1111000, // 0x78
    BLB_CLI = 0b_1111001, // 0x79
    WSBAPP = 0b_1111010, // 0x7A
    BLBUI = 0b_10000000, // 0x80
    USN = 0b_10000001, // 0x81
    USERMODE_VOLSNAP = 0b_10000010, // 0x82
    TIERING = 0b_10000011, // 0x83
    WSB_ONLINE = 0b_10000101, // 0x85
    ONLINE_ID = 0b_10000110, // 0x86
    DLS = 0b_10011001, // 0x99
    SOS = 0b_10100000, // 0xA0
    DEBUGGERS = 0b_10110000, // 0xB0
    USERMODE_SPACES = 0b_11100111, // 0xE7
    DMSERVER = 0b_1_00000000, // 0x100
    RESTORE = 0b_1_00000000, // 0x100
    SPP = 0b_1_00000000, // 0x100
    DEPLOYMENT_SERVICES_SERVER = 0b_1_00000001, // 0x101
    DEPLOYMENT_SERVICES_IMAGING = 0b_1_00000010, // 0x102
    DEPLOYMENT_SERVICES_MANAGEMENT = 0b_1_00000011, // 0x103
    DEPLOYMENT_SERVICES_UTIL = 0b_1_00000100, // 0x104
    DEPLOYMENT_SERVICES_BINLSVC = 0b_1_00000101, // 0x105
    DEPLOYMENT_SERVICES_PXE = 0b_1_00000111, // 0x107
    DEPLOYMENT_SERVICES_TFTP = 0b_1_00001000, // 0x108
    DEPLOYMENT_SERVICES_TRANSPORT_MANAGEMENT = 0b_1_00010000, // 0x110
    DEPLOYMENT_SERVICES_DRIVER_PROVISIONING = 0b_1_00010110, // 0x116
    DEPLOYMENT_SERVICES_MULTICAST_SERVER = 0b_1_00100001, // 0x121
    DEPLOYMENT_SERVICES_MULTICAST_CLIENT = 0b_1_00100010, // 0x122
    DEPLOYMENT_SERVICES_CONTENT_PROVIDER = 0b_1_00100101, // 0x125
    LINGUISTIC_SERVICES = 0b_1_00110001, // 0x131
    WEB = 0b_11_01110101, // 0x375
    WEB_SOCKET = 0b_11_01110110, // 0x376
    AUDIOSTREAMING = 0b_100_01000110, // 0x446
    ACCELERATOR = 0b_110_00000000, // 0x600
    MOBILE = 0b_111_00000001, // 0x701
    WMAAECMA = 0b_111_11001100, // 0x7CC
    
    /*
    WEP = 0b_1000_00000001, // 0x801
    SYNCENGINE = 0b_1000_00000010, // 0x802
    DIRECTMUSIC = 0b_1000_01111000, // 0x878
    DIRECT3D10 = 0b_1000_01111001, // 0x879
    DXGI = 0b_1000_01111010, // 0x87A
    DXGI_DDI = 0b_1000_01111011, // 0x87B
    DIRECT3D11 = 0b_1000_01111100, // 0x87C
    LEAP = 0b_1000_10001000, // 0x888
    AUDCLNT = 0b_1000_10001001, // 0x889
    WINCODEC_DWRITE_DWM = 0b_1000_10011000, // 0x898
    DIRECT2D = 0b_1000_10011001, // 0x899
    DEFRAG = 0b_1001_00000000, // 0x900
    USERMODE_SDBUS = 0b_1001_00000001, // 0x901
    JSCRIPT = 0b_1001_00000010, // 0x902
    PIDGENX = 0b_1010_00000001, // 0xA01
    */
}