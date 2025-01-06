using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PruebaTecnica_NetCore
{
	class Program
	{
		static HttpClient client = new HttpClient();

		static async Task<List<CatBreed>> ObtenerGatos(string path)
		{
			try
			{
				List<CatBreed> gatos = null;
				HttpResponseMessage response = await client.GetAsync(path);
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					gatos = JsonConvert.DeserializeObject<List<CatBreed>>(json);
				}
				return gatos;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		
		static async Task Main(string[] args)
		{
			try
			{
				Console.WriteLine("Obteniendo...");
				List<CatBreed> razas = await ObtenerGatos("https://api.thecatapi.com/v1/breeds");

				if(razas != null)
				{
					Console.WriteLine("Las 10 razas de gatos mas inteligentes:");

					var razasInteligentes = razas.OrderByDescending(x => x.intelligence).Take(10);
					var promedio = Math.Round(razas.Average(r => r.adaptability),2);

					foreach (var raza in razasInteligentes)
					{
						Console.WriteLine($"Nombre: {raza.name}");
						Console.WriteLine($"Descripción: {raza.description}");
						Console.WriteLine($"País de origen: {raza.origin ?? "No disponible"}");
						Console.WriteLine($"Nivel de inteligencia: {raza.intelligence}");
					}
						Console.WriteLine($"Nivel de adaptabilidad de las razas obtenidas es: {promedio}");
				}
				else
				{
					Console.WriteLine("No se pudo obtener la lista");
				}

			}
			catch (HttpRequestException)
			{
				Console.WriteLine("Hubo un problema al intentar conectarse al servidor.");
			}
			catch (JsonException)
			{
				Console.WriteLine("Ocurrió un error al procesar los datos obtenidos del JSON");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ocurrió un error inesperado: " + ex.ToString());
			}

		}
	}
}
