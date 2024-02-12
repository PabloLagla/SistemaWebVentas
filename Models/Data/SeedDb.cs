
using ProyectoEcommerce.Enums;
using ProyectoEcommerce.Migrations;
using ProyectoEcommerce.Models.Entidades;
using ProyectoEcommerce.Services;
using System.Net;

namespace ProyectoEcommerce.Models.Data
{
    public class SeedDb
    {
        private readonly EcommerceContext _context;
        private readonly IServicioUsuario _servicioUsuario;

        public SeedDb(EcommerceContext context, IServicioUsuario servicioUsuario)
        {
            _context = context;
            _servicioUsuario = servicioUsuario;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await VerificarCategoriasAsync();           
            await VerificarRolesAsync();
            await VerificarUsuariosAsync("Administrador", "admin@gmail.com", "3170 2187", 
            TipoUsuario.Administrador);
        }

        

        private async Task<Usuario> VerificarUsuariosAsync(string nombre, string email, string telefono, TipoUsuario tipoUsuario)
        {
            Usuario usuario = await _servicioUsuario.ObtenerUsuario(email);
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = nombre,                   
                    Email = email,
                    UserName = email,
                    PhoneNumber = telefono,                                       
                    TipoUsuario = tipoUsuario,
                };
                await _servicioUsuario.CrearUsuario(usuario, "123456");
                await _servicioUsuario.AsignarRol(usuario, tipoUsuario.ToString());
            }
            return usuario;
        }

        private async Task VerificarRolesAsync()
        {
            await _servicioUsuario.VerificarRol(TipoUsuario.Administrador.ToString());
            await _servicioUsuario.VerificarRol(TipoUsuario.Cliente.ToString());
        }

        private async Task VerificarCategoriasAsync()
        {
            if(!_context.Categorias.Any())
            {
                _context.Categorias.Add(new Categoria { Nombre = "Combo" });
                _context.Categorias.Add(new Categoria { Nombre = "Entrada" });
                _context.Categorias.Add(new Categoria { Nombre = "Arroz" });
                _context.Categorias.Add(new Categoria { Nombre = "Tallarin" });
                _context.Categorias.Add(new Categoria { Nombre = "Sopa" });
            }

            await _context.SaveChangesAsync();
        }
    }
}
