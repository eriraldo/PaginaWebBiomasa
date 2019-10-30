using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectWebPage.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web.Helpers;
using System.Net;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;

namespace ProjectWebPage
{
    public class ProductorController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: Productor
        public ActionResult Productor()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Productor( Login login)
        {
            Login nuevo = new Login();
                nuevo.usuario= login.usuario;
                nuevo.contrasena = login.contrasena;
                nuevo.idUsuario = "1";

                var myDict = new Dictionary<string, string>
        {
            { "user", nuevo.usuario },
            { "password", nuevo.contrasena },
            { "idProductor", nuevo.idUsuario }
        };

                JObject x = await loginProductor(myDict);
            System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
            if (x.GetValue("status").ToString() == "True" && x.GetValue("idProductor").ToString() == "1")
                {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return RedirectToAction("MenuProductor");
                }
                else
                {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return View("~/Views/Home/home.cshtml");
                }
            
            

        }

        public async Task<JObject> loginProductor(Dictionary<String,String> json)
        {
            

            var content = new FormUrlEncodedContent(json);
            var response = await client.PostAsync("https://biomasa.herokuapp.com/login", content);
            var responseString = await response.Content.ReadAsStringAsync();
            JObject jsonAnswer = JObject.Parse(responseString);
            return jsonAnswer;
        }

        

        [HttpGet]
        public async Task<ActionResult> MostrarRegistroBiomasa()
        {
            string dr = await client.GetStringAsync("https://biomasa.herokuapp.com/reportOrden");
            JArray newJson = JArray.Parse(dr);
            
            DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(newJson));
            //string html = ConvertDataTableToHTML(dataTable);
            return View(dataTable);
        }

        

        public ActionResult NuevoUsuario()
        {

            return View();
        }
        public ActionResult MenuProductor()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MenuProductor(ProductorClass clase)
        {
            ProductorClass nuevo = new ProductorClass();
            nuevo.Cantidad = clase.Cantidad;
            nuevo.Estado = clase.Estado;
            nuevo.NombreProductor = clase.NombreProductor;

            var myDict = new Dictionary<string, string>
        {
            { "cantidad", nuevo.Cantidad.ToString() },
            { "estado", nuevo.Estado },
            { "nombreProductor", nuevo.NombreProductor }
        };

            JObject x = await crearMenuProdu(myDict);
            System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
            if (x.GetValue("status").ToString() == "True" || x.GetValue("status").ToString() == "true")
            {
                System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
                return RedirectToAction("MostrarRegistroBiomasa");
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
            var response = await client.PostAsync("https://biomasa.herokuapp.com/newOrden", content);
            var responseString = await response.Content.ReadAsStringAsync();
            JObject jsonAnswer = JObject.Parse(responseString);
            return jsonAnswer;
        }


        [HttpPost]
        public async Task<ActionResult> NuevoUsuario(Login login)
        {
            Login nuevo = new Login();
            string value = Request.Form["idProductor"];
            nuevo.usuario = login.usuario;
            nuevo.contrasena = login.contrasena;
            nuevo.idUsuario = value;
            
            var myDict = new Dictionary<string, string>
        {
            { "user", nuevo.usuario },
            { "password", nuevo.contrasena },
            { "idProductor", value }

        };

                JObject x = await crearUser(myDict);
                //System.Diagnostics.Debug.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                //System.Diagnostics.Debug.WriteLine(x.GetValue("status").ToString());
           //System.Diagnostics.Debug.WriteLine(nuevo.contrasena);
           // System.Diagnostics.Debug.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            if (x.GetValue("status").ToString() == "True")
                {
                System.Diagnostics.Debug.WriteLine("se agrego el usuario a la BD correctamente");
                return View("~/Views/Home/home.cshtml");
                }
                else
                {
                System.Diagnostics.Debug.WriteLine(x);
                return RedirectToAction("Productor");
                }
            
            
            
        }



        public async Task<JObject> crearUser(Dictionary<String, String> json)
        {
            var values = new Dictionary<String, String>
            {
                {"user", "prueba"},
                 {"password", "prueba"}
            };

            var content = new FormUrlEncodedContent(json);
            var response = await client.PostAsync("https://biomasa.herokuapp.com/newProductor", content);
            var responseString = await response.Content.ReadAsStringAsync();
            //System.Diagnostics.Debug.WriteLine(responseString);
            JObject jsonAnswer = JObject.Parse(responseString);
            return jsonAnswer;
        }

        //var responseString = await client.GetStringAsync("http://www.example.com/recepticle.aspx"); -> GET
    
    
    }
}