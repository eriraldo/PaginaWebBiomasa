using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectWebPage.Models;
using System.Net.Http;
using System.Threading.Tasks;

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


        public async Task<ActionResult> MenuProductor(Login login)
        {
            
            string user = login.usuario;
            string pass = login.contrasena;
            int id = login.idUsuario;

            //string x = await makePostAsync(user);
            Console.WriteLine(x);
            
            return View();
        }

        public async Task<string> makePostAsync(string json)
        {
            var values = new Dictionary<String, String>
            {
                {"prueba", "prueba"}
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://172.20.10.2:5000/loginProductor", content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

    }
}