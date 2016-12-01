using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace XamarinDiplomado
{
	[DataTable("TodoItem")]
	public class ToDoItem
	{
		[JsonProperty("Id")]
		public string Id { get; set;}

		[JsonProperty("Titulo")]
		public string Titulo { get; set;}

		[JsonProperty("Descripcion")]
		public string Descripcion { get; set; }

		[Version]
		public string Version { get; set;}
	}
}
