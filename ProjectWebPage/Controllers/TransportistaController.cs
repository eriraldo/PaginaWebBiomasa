using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectWebPage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectWebPage.Controllers
{
    public class TransportistaController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        // GET: Transportista
        public ActionResult Transportista()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Transportista(Login login)
        {
            Login nuevo = new Login();
            nuevo.usuario = login.usuario;
            nuevo.contrasena = login.contrasena;
            nuevo.idUsuario = login.idUsuario;

            var myDict = new Dictionary<string, string>
        {
            { "user", nuevo.usuario },
            { "password", nuevo.contrasena },
            { "idProductor", nuevo.idUsuario }
        };

            JObject x = await loginTransportista(myDict);
            //System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
            if (x.GetValue("status").ToString() == "True" && x.GetValue("idProductor").ToString() == "2")
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return RedirectToAction("MenuTrans");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return View("~/Views/Home/home.cshtml");
            }



        }

        public async Task<JObject> loginTransportista (Dictionary<String, String> json)
        {
            /*var values = new Dictionary<String, String>
            {
                {"prueba", "prueba"}
            };*/

            var content = new FormUrlEncodedContent(json);
            var response = await client.PostAsync("https://biomasa.herokuapp.com/login", content);
            var responseString = await response.Content.ReadAsStringAsync();
            JObject jsonAnswer = JObject.Parse(responseString);
            return jsonAnswer;
        }

        public ActionResult MenuTrans()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> MenuTrans(TransportistaClass clase)
        {
            TransportistaClass nuevo = new TransportistaClass();
            nuevo.IdPaquete = clase.IdPaquete;
            nuevo.Entregado = clase.Entregado;
            nuevo.Recibido = clase.Recibido;
            string entregadoStr = Request.Form["fechaEntregado"].ToString();
            string recibidoStr = Request.Form["fechaRecibido"].ToString();
            System.Diagnostics.Debug.WriteLine(entregadoStr.ToString());
            var myDict = new Dictionary<string, string>
        {
            { "codigoPaquete", nuevo.IdPaquete.ToString() },
            { "fechaRecibido", entregadoStr },
            { "fechaEntregado", recibidoStr }
            

        };

            JObject x = await crearMenuProdu(myDict);
            System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
            if (x.GetValue("status").ToString() == "True" || x.GetValue("status").ToString() == "true")
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return RedirectToAction("MostrarViajes");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return View("~/Views/Home/home.cshtml");
            }



        }

        public async Task<JObject> crearMenuProdu(Dictionary<String, String> json)
        {


            var content = new FormUrlEncodedContent(json);
            var response = await client.PostAsync("https://biomasa.herokuapp.com/newPaquete", content);
            var responseString = await response.Content.ReadAsStringAsync();
            JObject jsonAnswer = JObject.Parse(responseString);
            return jsonAnswer;
        }

        public async Task<ActionResult> MostrarViajes()
        {

            string dr = await client.GetStringAsync("https://biomasa.herokuapp.com/reportPaquete");
            JArray newJson = JArray.Parse(dr);

            DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(newJson));
            


            return View(dataTable);
        }

        

        public ActionResult NuevoUsuario()
        {
            return View("~/Views/Productor/NuevoUsuario.cshtml");
        }




    }
}