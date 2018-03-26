﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace F18IDABH2Gr30CosmosDB
{
	class Program
	{
		private const string EndpointUrl = "https://localhost:8081";

		private const string PrimaryKey =
			"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

		private DocumentClient client;

		static void Main(string[] args)
		{
			try
			{
				Program p = new Program();
				p.GetStartedDemo().Wait();
			}
			catch (DocumentClientException de)
			{
				Exception baseException = de.GetBaseException();
				Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
			}
			catch (Exception e)
			{
				Exception baseException = e.GetBaseException();
				Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
			}
			finally
			{
				Console.WriteLine("End of demo, press any key to exit.");
				Console.ReadKey();
			}
		}

		private async Task GetStartedDemo()
		{
			this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
			await this.client.CreateDatabaseIfNotExistsAsync(new Database {Id = "PersonKartotekDB"});
			await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("PersonKartotekDB"),
				new DocumentCollection {Id = "PersonCollection"});

			Address aseAddress = new Address
			{
				AddressType = "Work",
				Street = "Finlandsgade",
				StreetNumber = 21,
				ApartmentNumber = 1,
				ZIP = new ZIP
				{
					Country = "Denmark",
					State = "Jylland",
					Town = "Aarhus V",
					ZipCode = "8210"
				}
			};

			Person nickolaiPerson = new Person
			{
				PersonId = "Nickolai.1",
				FirstName = "Nickolai",
				MiddleName = "Lundby",
				LastName = "Ernst",
				PersonType = "Co-worker",
				Email = "nicko@minmail.dk",
				Addresses = new Address[]
				{
					new Address
					{
						AddressType = "Home",
						Street = "Vestre Ringgade",
						StreetNumber = 176,
						ApartmentNumber = 4,
						ZIP = new ZIP
						{
							Country = "Denmark",
							State = "Jylland",
							Town = "Aarhus C",
							ZipCode = "8000"
						}
					},
					aseAddress
				},
				PhoneNumbers = new PhoneNumber[]
				{
					new PhoneNumber
					{
						Number = 12345678,
						PhoneType = "Private",
						Provider = "TDC"
					},
					new PhoneNumber
					{
						Number = 87654321,
						PhoneType = "Work",
						Provider = "Telia"
					}
				}
			};

			await this.CreateFamilyDocumentIfNotExists("PersonKartotekDB", "PersonCollection", nickolaiPerson);

			Person runePerson = new Person
			{
				PersonId = "Rune.1",
				FirstName = "Rune",
				MiddleName = "Aagaard",
				LastName = "Keena",
				PersonType = "Co-worker",
				Email = "rune@minmail.dk",
				Addresses = new Address[]
				{
					new Address
					{
						AddressType = "Home",
						Street = "Abildgade",
						StreetNumber = 21,
						ApartmentNumber = 1,
						ZIP = new ZIP
						{
							Country = "Denmark",
							State = "Jylland",
							Town = "Aarhus N",
							ZipCode = "8200"
						}
					},
					aseAddress
				},
				PhoneNumbers = new PhoneNumber[]
				{
					new PhoneNumber
					{
						Number = 12345678,
						PhoneType = "Private",
						Provider = "TDC"
					},
					new PhoneNumber
					{
						Number = 87654321,
						PhoneType = "Work",
						Provider = "Telia"
					}
				}
			};

			await this.CreateFamilyDocumentIfNotExists("PersonKartotekDB", "PersonCollection", runePerson);
		}

		private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
		{
			Console.WriteLine(format, args);
			Console.WriteLine("Press any key to continue ...");
			Console.ReadKey();
		}

		public class Person
		{
			[JsonProperty(PropertyName = "id")] public string PersonId { get; set; }
			public string FirstName { get; set; }
			public string MiddleName { get; set; }
			public string LastName { get; set; }
			public string PersonType { get; set; }
			public string Email { get; set; }
			public Address[] Addresses { get; set; }
			public PhoneNumber[] PhoneNumbers { get; set; }

			public override string ToString()
			{
				return JsonConvert.SerializeObject(this);
			}
		}

		public class Address
		{
			public string AddressType { get; set; }
			public string Street { get; set; }
			public uint StreetNumber { get; set; }
			public uint ApartmentNumber { get; set; }
			public ZIP ZIP { get; set; }
			//public Person[] Persons { get; set; }
		}

		public class PhoneNumber
		{
			public string PhoneType { get; set; }
			public uint Number { get; set; }
			public string Provider { get; set; }
		}

		public class ZIP
		{
			public string Country { get; set; }
			public string Town { get; set; }
			public string State { get; set; }
			public string ZipCode { get; set; }
		}

		public class ZIPList
		{
			public ZIP[] Zips { get; set; }
		}

		private async Task CreateFamilyDocumentIfNotExists(string databaseName, string collectionName, Person person)
		{
			try
			{
				await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, person.PersonId));
				this.WriteToConsoleAndPromptToContinue("Found {0}", person.PersonId);
			}
			catch (DocumentClientException de)
			{
				if (de.StatusCode == HttpStatusCode.NotFound)
				{
					await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), person);
					this.WriteToConsoleAndPromptToContinue("Created Person {0}", person.PersonId);
				}
				else
				{
					throw;
				}
			}
		}

	}
}
