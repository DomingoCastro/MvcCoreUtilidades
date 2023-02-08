using Azure;
using Azure.Data.Tables;
using MvcCoreUtilidades.Models;

namespace MvcCoreUtilidades.Services
{
    public class ServiceStorageTables
    {
        private TableClient tableClient;
        public ServiceStorageTables(string azureKeys)
        {
            TableServiceClient serviceClient= new TableServiceClient(azureKeys);
            this.tableClient = serviceClient.GetTableClient("clientes");
            //CREAMOS LA TABLA SI NO EXISTE
            this.tableClient.CreateIfNotExists();
        }

        public async Task CreateClienteAsync(string id, string empresa, string nombre, int salario, int edad)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Empresa = empresa,
                Nombre = nombre,
                Salario = salario,
                Edad = edad
            };
            await this.tableClient.AddEntityAsync<Cliente>(cliente);
        }
        //BUSCAR UN CLIENTE con ROWKEY Y PARTITIONKEY
        public async Task<Cliente> FindClientAsync(string partitionKey, string rowKey)
        {
            Cliente cliente = await this.tableClient.GetEntityAsync<Cliente>(partitionKey, rowKey);
            return cliente;
        }

        //DELETE CLIENTE CON PARTITION KEY Y ROW KEY
        public async Task DeleteClientAsync(string partitionKey, string rowKey)
        {
            await this.tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        //RECUPERAR TODOS LOS CLIENTES

        public async Task<List<Cliente>> GetClientesAsync()
        {
            //NECESITAMOS UNA COLECCION DE CLIENTES PARA ALMACENARLOS
            List<Cliente> clientes= new List<Cliente>();
            var query = this.tableClient.QueryAsync<Cliente>(filter: "");
            await foreach(Cliente item in query)
            {
                clientes.Add(item);
            }
            return clientes;
        }

        //RECUPERAMOS CLIENTES POR EMPRESA

        public List<Cliente> GetClientesEmpresas(string empresa)
        {
            Pageable<Cliente> query = this.tableClient.Query<Cliente>(x => x.Empresa == empresa);
            return query.ToList();
        }

    }
}
