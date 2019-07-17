using Newtonsoft.Json;
using System;
using System.Management.Automation;
using WaykDen.Controllers;
using WaykDen.Models;

namespace WaykDen.Cmdlets
{
    [Cmdlet(VerbsCommon.Set, "WaykDenUserLicense"), CmdletBinding(DefaultParameterSetName="ByUserID")]
    public class SetWaykDenUserLicense : RestApiCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ByUserID", HelpMessage = "ID of a user.")]
        public string UserID {get; set;}
        [Parameter(Mandatory = true, ParameterSetName = "ByUsername", HelpMessage = "Username of a user.")]
        public string Username {get; set;}
        [Parameter(ParameterSetName = "ByUsername", HelpMessage = "License serial")]
        [Parameter(ParameterSetName = "ByUserID", HelpMessage = "License serial.")]
        public string Serial {get; set;}
        [Parameter(ParameterSetName = "ByUsername", HelpMessage = "ID of a license.")]
        [Parameter(ParameterSetName = "ByUserID", HelpMessage = "ID of a license.")]
        public string LicenseID {get; set;}
        protected override void ProcessRecord()
        {
            string data = string.Empty;

            if(this.ParameterSetName == "ByUsername")
            {
                data = JsonConvert.SerializeObject(new ByUsernameLicenseObject{username = this.Username, serial_number = this.Serial});
                this.DenRestAPIController.PutUser(data);
            }
            else
            {
                if(!string.IsNullOrEmpty(this.LicenseID))
                {
                    data = JsonConvert.SerializeObject(new ByIDObject{license_id = this.LicenseID});
                }
                else if(!string.IsNullOrEmpty(this.Serial))
                {
                    data = JsonConvert.SerializeObject(new BySerialObject{serial_number = this.Serial});
                } else return;

                this.DenRestAPIController.PutUser(data, $"/{this.UserID}");
            }
        }
    }
}