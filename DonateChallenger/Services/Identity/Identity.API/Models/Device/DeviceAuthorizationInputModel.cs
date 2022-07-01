using Identity.API.Models.Consent;

namespace Identity.API.Models.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}