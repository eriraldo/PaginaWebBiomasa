using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectWebPage.Models;

namespace ProjectWebPage.Controllers
{
    public class EmpleadoController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        public ActionResult Empleado ()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Empleado(Login login)
        {
            Login nuevo = new Login();
            nuevo.usuario = login.usuario;
            nuevo.contrasena = login.contrasena;
            nuevo.idUsuario = login.idUsuario;

            var myDict = new Dictionary<string, string>
        {
            { "user", nuevo.usuario },
            { "password", nuevo.contrasena },
            { "idProductor", nuevo.idUsuario}
        };

            JObject x = await loginEmpleado(myDict);
            //System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
            if (x.GetValue("status").ToString() == "True" && x.GetValue("idProductor").ToString() == "3")
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return RedirectToAction("MenuEmpleado");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return View("~/Views/Home/home.cshtml");
            }



        }

        public async Task<JObject> loginEmpleado (Dictionary<String, String> json)
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
        public ActionResult MenuEmpleado()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MenuEmpleado(EmpleadoClass clase)
        {
            EmpleadoClass nuevo = new EmpleadoClass();
            nuevo.CantidadBioabono = clase.CantidadBioabono;
            nuevo.CantidadEnergia = clase.CantidadEnergia;
            nuevo.biomasaUsada = clase.biomasaUsada;
            nuevo.costo = clase.costo;


            var myDict = new Dictionary<string, string>
        {
            { "cantidadAbono", nuevo.CantidadBioabono.ToString() },
            { "cantidadEnergia", nuevo.CantidadEnergia.ToString() },
            { "biomasaUsada", nuevo.biomasaUsada.ToString() },
            { "costo", nuevo.costo.ToString()}

        };

            JObject x = await crearMenuProdu(myDict);
            System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
            if (x.GetValue("status").ToString() == "True" || x.GetValue("status").ToString() == "true")
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return RedirectToAction("MostrarProduccion");
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
            var response = await client.PostAsync("https://biomasa.herokuapp.com/newProduccion", content);
            var responseString = await response.Content.ReadAsStringAsync();
            JObject jsonAnswer = JObject.Parse(responseString);
            return jsonAnswer;
        }
        public async Task<ActionResult> MostrarProduccion()
        {
            string dr = await client.GetStringAsync("https://biomasa.herokuapp.com/reportProduccion");
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