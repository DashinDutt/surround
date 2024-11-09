param (
    [string]$deviceId  # The device ID passed as an argument
)

# Add the necessary types for COM interaction
Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;

public class PolicyConfig
{
    [DllImport("ole32.dll", CharSet = CharSet.Auto)]
    public static extern int CoCreateInstance(ref Guid clsid, IntPtr pUnkOuter, uint dwClsContext, ref Guid iid, out IPolicyConfig config);
    
    [ComImport]
    [Guid("f8679f50-850a-41cf-9c72-430f290290c8")]
    public interface IPolicyConfig
    {
        void SetDefaultEndpoint([MarshalAs(UnmanagedType.LPWStr)] string deviceId, int role);
    }
}
"@

# GUID for the PolicyConfigClient COM object (use the Guid constructor directly)
$CLSID_PolicyConfigClient = [guid]"f8679f50-850a-41cf-9c72-430f290290c8"
$IID_IPolicyConfig = [guid]"f8679f50-850a-41cf-9c72-430f290290c8"

# Declare the variable to store the interface implementation
$policyConfigClient = $null

# CoCreate an instance of the PolicyConfigClient COM object
$result = [PolicyConfig]::CoCreateInstance([ref]$CLSID_PolicyConfigClient, [IntPtr]::Zero, 1, [ref]$IID_IPolicyConfig, [ref]$policyConfigClient)

# Check if instance creation was successful
if ($result -ne 0) {
    Write-Output "Failed to create PolicyConfigClient instance. Error Code: $result"
    exit
}

# Set the default endpoint device for multimedia (audio output for media)
$role = 1  # 0 for Console, 1 for Multimedia (audio output for media), 2 for Communications
$policyConfigClient.SetDefaultEndpoint($deviceId, $role)

Write-Output "Device $deviceId has been set as the default audio output device for media."
