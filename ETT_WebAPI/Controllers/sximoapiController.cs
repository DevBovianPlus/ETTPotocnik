using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.Models.Location;
using ETT_DAL.Models.MobileTransaction;
using ETT_DAL.Models.Product;
using ETT_DAL.Models.Supplier;
using ETT_DAL.Models.Users;
using ETT_WebAPI.Common;
using ETT_WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ETT_WebAPI.Controllers
{
    public class sximoapiController : ApiController
    {
        /// <summary>
        /// Na podlagi query stringa metoda vrne zapise iz podatkovne baze
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetModule(string module)
        {
            //var model = new ResponseContentModel<List<T>>();

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            var result = CommonMethods.AuthenticateWebAPI(token);

            if (result != WebAPIEnums.AuthenticationStatus.OK)
                return Json(CommonMethods.GetAutheticationError(result));
            else
            {
                if (module == WebAPIEnums.Module.inventory.ToString())
                {
                    var model = new ResponseContentModel<List<ProductAPIModel>>();
                    IProductRepository productRepo = new ProductRepository();

                    model.rows = productRepo.GetProductsMobile();
                    model.total = model.rows.Count.ToString();
                    model.key = "id";
                    model.control = new Control { page = "1", order = "asc", sort = "", limit = "" };
                    return Json(model);
                }
                else if (module == WebAPIEnums.Module.location.ToString())
                {
                    var model = new ResponseContentModel<List<LocationAPIModel>>();
                    ILocationRepository locRepo = new LocationRepository();
                    model.rows = locRepo.GetLocationsMobile();
                    model.total = model.rows.Count.ToString();
                    model.key = "id";
                    model.control = new Control { page = "1", order = "asc", sort = "", limit = "" };
                    return Json(model);
                }
                else if (module == WebAPIEnums.Module.supplier.ToString())
                {
                    var model = new ResponseContentModel<List<SupplierAPIModel>>();
                    IClientRepository clientRepo = new ClientRepository();
                    model.rows = clientRepo.GetSuppliersMobile();
                    model.total = model.rows.Count.ToString();
                    model.key = "id";
                    model.control = new Control { page = "1", order = "asc", sort = "", limit = "" };
                    return Json(model);
                }
                else if (module == WebAPIEnums.Module.users.ToString())
                {
                    var model = new ResponseContentModel<List<UserAPIModel>>();
                    IUserRepository userRepo = new UserRepository();
                    model.rows = userRepo.GetUsersMobile();
                    model.total = model.rows.Count.ToString();
                    model.key = "id";
                    model.control = new Control { page = "1", order = "asc", sort = "", limit = "" };
                    return Json(model);
                }
            }

            return Json(CommonMethods.GetAutheticationError(WebAPIEnums.AuthenticationStatus.FAILED));
        }

        [HttpPost]
        public IHttpActionResult MobileTransaction(string module, [FromBody] object mobileTransaction)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            var result = CommonMethods.AuthenticateWebAPI(token);

            if (result != WebAPIEnums.AuthenticationStatus.OK)
                return Json(CommonMethods.GetAutheticationError(result));
            else if(result == WebAPIEnums.AuthenticationStatus.OK && module == "mobiletransactions")
            {
                try
                {
                    var transaction = JsonConvert.DeserializeObject<MobileTransactionAPIModel>(mobileTransaction.ToString());

                    IMobileTransactionRepository mobileRepo = new MobileTransactionRepository();
                    mobileRepo.SaveMobileTransaction(transaction);

                    /*string path = AppDomain.CurrentDomain.BaseDirectory + "/Uploads";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var provider = new MultipartFormDataStreamProvider(path);
                    await Request.Content.ReadAsMultipartAsync(provider);
                    var transaction = GetMobileTransactionValues(provider);*/

                }
                catch (Exception ex)
                {
                    return BadRequest("Failed");
                }

            }

            return Json("Success");
        }

        private MobileTransactionAPIModel GetMobileTransactionValues(MultipartFormDataStreamProvider provider)
        {
            Type mobileTrans = typeof(MobileTransactionAPIModel);
            MobileTransactionAPIModel model = new MobileTransactionAPIModel();
            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    PropertyInfo info = mobileTrans.GetProperty(key);
                    if (info.PropertyType == typeof(DateTime))
                    {
                        DateTime parsedDate;
                        string pattern = "yyyy-MM-dd 0HH:mm:ss";
                        DateTime.TryParseExact(val, pattern, null, DateTimeStyles.None, out parsedDate);
                        info.SetValue(model, parsedDate);
                    }
                    else
                        info.SetValue(model, Convert.ChangeType(val, info.PropertyType));
                }
            }

            return model;
        }
    }
}
