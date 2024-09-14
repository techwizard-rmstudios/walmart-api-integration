
/// *************************************************************
/// *             Coded by Ekaterina Bozhko                     *
/// *             bozhkokateryna12@gmail.com                    *
/// *************************************************************

using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static WalmartAPI.WalmartAuth;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace WalmartAPI
{
    public class InventoryManager
    {
        public async Task<Inventory> GetInventory(string accessToken, string sku)
        {
            var client = GetHttpClient(accessToken);
            var endpoint = $"https://marketplace.walmartapis.com/v3/inventory?sku={sku}";

            HttpResponseMessage responseMessage = null;
            Inventory response = null;
            try
            {
                responseMessage = await client.GetAsync(endpoint);
                responseMessage.EnsureSuccessStatusCode();
                response = await responseMessage.Content.ReadAsAsync<Inventory>();
                MessageBox.Show("Success!", "WalmartAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WalmartAPI");
            }
            finally
            {
                responseMessage?.Dispose();
                client.Dispose();
            }

            return response;
        }
        public async Task<Inventory> PutInventory(string accessToken, string sku, string unit, int amount)
        {
            var client = GetHttpClient(accessToken);
            var endpoint = $"https://marketplace.walmartapis.com/v3/inventory?sku={sku}";

            Inventory schema = new Inventory(sku, unit, amount);

            var content = new StringContent(JsonConvert.SerializeObject(schema), null, "application/json");

            HttpResponseMessage responseMessage = null;
            Inventory response = null;
            try
            {
                responseMessage = await client.PutAsync(endpoint, content);
                responseMessage.EnsureSuccessStatusCode();
                response = await responseMessage.Content.ReadAsAsync<Inventory>();
                MessageBox.Show("Success!", "WalmartAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WalmartAPI");
            }
            finally
            {
                responseMessage?.Dispose();
                client.Dispose();
            }

            return response;
        }

        public async Task<Feed> PostInventory(string accessToken, string path)
        {
            var client = GetHttpClient(accessToken);
            var endpoint = "https://marketplace.walmartapis.com/v3/feeds?feedType=inventory";

            MultipartFormDataContent content = new MultipartFormDataContent();

            byte[] fileContent = File.ReadAllBytes(path);
            ByteArrayContent fileContentPart = new ByteArrayContent(fileContent);
            fileContentPart.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = Path.GetFileName(path)
            };
            fileContentPart.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Add(fileContentPart);

            HttpResponseMessage responseMessage = null;
            Feed response = null;
            try
            {
                responseMessage = await client.PostAsync(endpoint, content);
                responseMessage.EnsureSuccessStatusCode();
                response = await responseMessage.Content.ReadAsAsync<Feed>();
                MessageBox.Show("Success!", "WalmartAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "WalmartAPI");
            }
            finally
            {
                responseMessage?.Dispose();
                client.Dispose();
            }

            return response;
        }

        private HttpClient GetHttpClient(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", accessToken);
            client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace Service");
            client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString("N"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }

    public class Quantity
    {
        public string unit { get; set; }
        public int amount { get; set; }

        public Quantity(string _unit, int _amount)
        {
            unit = _unit;
            amount = _amount;
        }
    }

    public class Inventory
    {
        public string sku { get; set; }
        public Quantity quantity { get; set; }

        public Inventory(string _sku, string _unit, int _amount)
        {
            sku = _sku;
            quantity = new Quantity(_unit, _amount);
        }
    }

    public class Feed
    {
        public string feedId { get; set; }
    }
}
