using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _contexto;
            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                _contexto.CarritoSesion.Add(carritoSesion);
                var res = await _contexto.SaveChangesAsync();



                if (res == 0)
                    throw new Exception("errores en la inserción del carrito");

                int id = carritoSesion.CarritoSesionId;

                foreach(var obj in request.ProductoLista)
                {

                    var detalleSesion = new CarritoSesionDetalle
                    {
                        CarritoSesionId = id,
                        FechaCreacion = DateTime.Now,
                        ProductoSeleccionado = obj
                    };
                    _contexto.CarritoSesionDetalle.Add(detalleSesion); 
                }
                res = await _contexto.SaveChangesAsync();

                if (res > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("no se pudo insertar detalle");
            }
        }
    }
}
