﻿using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;
namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {
        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });


            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();
            var dbset = new Mock<DbSet<LibreriaMaterial>>();
            dbset.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbset.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbset.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbset.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());
            dbset.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken())).Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));


            dbset.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbset.Object);
            return contexto;
        }

        [Fact]
        public async void getLibroPorId()
        {
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg => 
            { 
                cfg.AddProfile(new MappingTest()); 
            
            });

            var mapper = mapConfig.CreateMapper();
            var request = new ConsultaFiltro.LibroUnico();

            request.LibroId = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);
            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);
        }

        [Fact]
        public async void GetLibros()
        {
            //System.Diagnostics.Debugger.Launch();
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingTest()));

            var mapper = mapConfig.CreateMapper();


            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());
        }

        [Fact]
        public async void GuardarLibro()
        {
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "libro de microservice";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro!= null);
        }
    }
}
