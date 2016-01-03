using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LazyLlamaLedger.Controllers
{
    [EnableCors("*", "*", "*")]
    public class SettingsController
        : ApiController
    {
        [HttpPost]
        [ActionName("ResetFileLocation")]
        public IHttpActionResult ResetFileLocation()
        {
            try
            {
                //Wipe the registry field
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

                key = key.OpenSubKey("LazyLlamaLedger", true);

                if (key != null)
                {
                    key.DeleteValue("DataPath");
                }

                return Ok("Deleted");
            }
            catch(Exception ex)
            {
                //Has it been deleted already?
                return InternalServerError(ex);
            }
        }
    }
}
